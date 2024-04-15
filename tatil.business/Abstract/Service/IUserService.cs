using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tatil.entity.EntityReferences.Identity.Create;
using tatil.entity.EntityReferences.User;
using tatil.entity.Identity;

namespace tatil.business.Abstract.Service
{
    public interface IUserService
    {
        List<AppUser> GetAdvertWaiters();
        List<AppUser> GetAdvertConfirmeds();
        Task<bool> CreateUser(UserCreateModel model);
        Task<bool> LoginAsync(string email, string password);
        Task<bool> SendAdvertRequestAsync(string email, string message);
        Task<bool> ConfirmAdvertRequestAsync(string userId);
        Task<bool> RejectAdvertRequestAsync(string userId);
        Task<bool> IsConfirmedForAdvert(string userName);
        Task<AppUser> GetUserForEdit(string userId);
        Task<bool> UpdateUser(AppUser user);
        Task<AppUser> GetUserProfile(string userName);
        Task<bool> ConfirmEmail(string userId, string token);
        Task<bool> UpdateEmailAsync(UpdateEmailModel model);
        Task<bool> ConfirmUpdateEmail(string userId, string token, string newEmail);
        Task<bool> SendUpdatePasswordRequestAsync(string userName);
        Task<bool> UpdatePassword(UpdatePasswordModel model);
        Task<bool> UpdateUserNames(string userName, string FirstName, string LastName);
        List<AppUser> GetAllUsersAsync();
        Task<bool> AddToRoleAsync(string userId, string roleId);
        Task<bool> SendResetPasswordRequest(string email);
        Task<bool> ResetPasswordAsync(ResetPasswordModel model);
    }
}
