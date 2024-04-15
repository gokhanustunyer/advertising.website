using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using tatil.business.Abstract.Service;
using tatil.data.Contexts;
using tatil.entity.EntityReferences.Identity.Create;
using tatil.entity.EntityReferences.User;
using tatil.entity.Identity;

namespace tatil.business.Concrete.Service
{
    public class UserService : IUserService
    {
        private readonly TatilDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailService _emailService;
        private readonly RoleManager<AppRole> _roleManager;
        public UserService(TatilDbContext context,
                           UserManager<AppUser> userManager,
                           SignInManager<AppUser> signInManager,
                           IEmailService emailService,
                           RoleManager<AppRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _roleManager = roleManager;
        }

        public async Task<bool> ConfirmAdvertRequestAsync(string userId)
        {
            AppUser user = await _userManager.FindByIdAsync(userId);
            user.isConfirmedForAdverts = true;
            user.isWaitingForAdverts = false;
            await _userManager.UpdateAsync(user);
            return true;
        }
        public async Task<bool> RejectAdvertRequestAsync(string userId)
        {
            AppUser user = await _userManager.FindByIdAsync(userId);
            user.isConfirmedForAdverts = false;
            user.isWaitingForAdverts = false;
            await _userManager.UpdateAsync(user);
            return true;
        }
        public async Task<bool> CreateUser(UserCreateModel model)
        {
            AppUser check = await _userManager.FindByEmailAsync(model.Email);
            if (check != null) { return false; }
            if (model.Password != model.PasswordAgn)
            {
                return false;
            }
            AppUser user = new AppUser()
            {
                Email = model.Email,
                UserName = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Id = Guid.NewGuid().ToString(),
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                isConfirmedForAdverts = false,
                isWaitingForAdverts = false,
                PhoneNumber = model.PhoneNumber,
            };
            IdentityResult result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                string url = $"Auth/ConfirmEmail?userId={user.Id}&token={HttpUtility.UrlEncode(token)}";
                await _emailService.SendEmailAsync(user.Email, "tatilimibuldum.com Hoş Geldiniz, Lütfen E-posta Adresinizi Doğrulayın", url);
                AppRole role = await _context.Roles.FirstOrDefaultAsync(r => r.NormalizedName == "USER");
                if (role != null)
                {
                    await AddToRoleAsync(user.Id, role.Id);
                }
            }
            return result.Succeeded;
        }

        public List<AppUser> GetAdvertWaiters()
            => _context.Users.Where(u => u.isWaitingForAdverts).ToList();

        public async Task<bool> LoginAsync(string email, string password)
        {
            AppUser user = await _userManager.FindByNameAsync(email);
            if (user == null) { return false; }
            if (!user.EmailConfirmed) { return false; }
            SignInResult result = await _signInManager.PasswordSignInAsync
                                         (email, password, true, false);
            return result.Succeeded;
        }

        public async Task<bool> SendAdvertRequestAsync(string userName, string message)
        {
            AppUser user =  await _userManager.FindByNameAsync(userName);
            user.isWaitingForAdverts = true;
            user.AdvertRequestMessage = message;
            await _userManager.UpdateAsync(user);
            return true;
        }

        public async Task<bool> IsConfirmedForAdvert(string userName)
        {
            AppUser user = await _userManager.FindByNameAsync(userName);
            return user.isConfirmedForAdverts;
        }

        public List<AppUser> GetAdvertConfirmeds()
            => _context.Users.Where(u => u.isConfirmedForAdverts).ToList();

        public async Task<AppUser> GetUserForEdit(string userId)
            => await _context.Users.Include(u => u.Advrets).ThenInclude(a => a.AdvertImages).FirstOrDefaultAsync(u => u.Id == userId);

        public async Task<bool> UpdateUser(AppUser userForEdit)
        {
            AppUser user = await _userManager.FindByNameAsync(userForEdit.UserName);
            user.PhoneNumber = userForEdit.PhoneNumber;
            user.FirstName = userForEdit.FirstName;
            user.LastName = userForEdit.LastName;
            user.isConfirmedForAdverts = userForEdit.isConfirmedForAdverts;
            await _userManager.UpdateAsync(user);
            return true;
        }

        public async Task<AppUser> GetUserProfile(string userName)
            => await _userManager.FindByNameAsync(userName);

        public async Task<bool> ConfirmEmail(string userId, string token)
        {
            IdentityResult result = new();
            AppUser user;
            if (userId != null && token != null)
            {
                user = await _userManager.FindByIdAsync(userId);
                result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    user.EmailConfirmed = true;
                    await _userManager.UpdateAsync(user);
                }
            }
            return result.Succeeded;
        }

        public async Task<bool> UpdateEmailAsync(UpdateEmailModel model)
        {
            if (model.NewEmail != model.NewEmailAgain) { return false; }
            AppUser user1 = await _userManager.FindByNameAsync(model.UserName);
            AppUser user2 = await _userManager.FindByEmailAsync(model.Email);
            if (user1 != user2) { return false; }
            bool pswCheck = await _userManager.CheckPasswordAsync(user1, model.Password);
            if (!pswCheck) { return false; }
            string token = await _userManager.GenerateChangeEmailTokenAsync(user1, model.NewEmail);
            string tokenUrl = $"User/ConfirmUpdateEmail?userId={user1.Id}&token={HttpUtility.UrlEncode(token)}&newEmail={model.NewEmail}";
            await _emailService.SendEmailAsync(model.NewEmail, "E-Posta Adresinizi Değiştirin", tokenUrl);

            return true;
        }
        public async Task<bool> ConfirmUpdateEmail(string userId, string token, string newEmail)
        {
            AppUser user = await _userManager.FindByIdAsync(userId);
            IdentityResult result = await _userManager.ChangeEmailAsync(user, newEmail, token);
            if (!result.Succeeded)
            {
                return false;
            }
            user.UserName = newEmail;
            await _userManager.UpdateAsync(user);
            await _userManager.UpdateSecurityStampAsync(user);
            await _signInManager.SignOutAsync();
            await _signInManager.SignInAsync(user, true);
            return true;
        }

        public async Task<bool> SendUpdatePasswordRequestAsync(string userName)
        {
            AppUser user = await _userManager.FindByNameAsync(userName);
            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
            string url = $"user/updatePassword/?token={HttpUtility.UrlEncode(token)}";
            await _emailService.SendEmailAsync(user.Email, "Şifre Yenileme", url);
            return true;
        }

        public async Task<bool> UpdatePassword(UpdatePasswordModel model)
        {
            if (model.NewPassword != model.NewPasswordAgain) { return true; }
            AppUser user = await _userManager.FindByNameAsync(model.UserName);
            bool pswCheck = await _userManager.CheckPasswordAsync(user, model.CurrentPassword);
            if (!pswCheck) { return true; }
            IdentityResult result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
            if (!result.Succeeded) { return true; }
            await _userManager.UpdateSecurityStampAsync(user);
            await _signInManager.SignOutAsync();
            await _signInManager.SignInAsync(user, true);
            return true;
        }

        public async Task<bool> UpdateUserNames(string userName, string FirstName, string LastName)
        {
            AppUser user = await _userManager.FindByNameAsync(userName);
            user.FirstName = FirstName;
            user.LastName = LastName;
            await _userManager.UpdateAsync(user);
            return true;
        }

        public List<AppUser> GetAllUsersAsync()
            => _userManager.Users.OrderByDescending(u => u.CreateDate).ToList();
        public async Task<bool> AddToRoleAsync(string userId, string roleId)
        {
            AppUser user = await _userManager.FindByIdAsync(userId);
            AppRole role = await _roleManager.FindByIdAsync(roleId);
            await _userManager.AddToRoleAsync(user, role.Name);
            return true;
        }

        public async Task<bool> SendResetPasswordRequest(string email)
        {
            AppUser user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return false;
            }
            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
            string url = $"Auth/ResetPassword?userId={user.Id}&token={HttpUtility.UrlEncode(token)}";
            await _emailService.SendEmailAsync(user.Email, "Şifretiniz Sıfırlayın - Arite", url);
            return true;
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordModel model)
        {
            AppUser user;
            IdentityResult result;
            if (model.Password != model.PasswordAgain)
            {
                return false;
            }
            user = await _userManager.FindByIdAsync(model.userId);
            if (user == null)
            {
                return false;
            }
            result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            return result.Succeeded;
        }
    }
}
