using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using tatil.business.Abstract.Service;
using tatil.entity.EntityReferences.Identity.Create;
using tatil.entity.EntityReferences.User;
using tatil.entity.Identity;

namespace tatil.webui.Controllers
{
    public class AuthController : Controller
    {
        private readonly IUserService _userService;
        private readonly SignInManager<AppUser> _signInManager;

        public AuthController(IUserService userService,
                              SignInManager<AppUser> signInManager)
        {
            _userService = userService;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet] public IActionResult Login()
        {
            return View();
        }
        [HttpPost] public async Task<IActionResult> Login([Required(ErrorMessage = "E-Posta Alanını Boş Geçemezsiniz")] string email, [Required(ErrorMessage = "Şifre Alanını Boş Geçemezsiniz")] string password)
        {
            if (ModelState.IsValid)
            {
                bool isLogged = await _userService.LoginAsync(email, password);
                if (!isLogged)
                {
                    TempData["Errors"] = "<li>Sistemde bu şekilde kayıtlı olan bir kullanıcı bulunmamakta</li>";
                    return View();
                }
                return Redirect("/");
            }
            return View();
        }
        [HttpGet] public IActionResult Register()
        {
            return View();
        }

        [HttpPost] public async Task<IActionResult> Register(UserCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Login");
            }
            bool isComplete = await _userService.CreateUser(model);
            if (!isComplete)
            {
                TempData["Errors"] = "Kayıt sırasında bir hata oluştur, seçitiğiniz e-posta adresi zaten kullanımda olabilir, Şifreniz kurallara uygun olmayabilir<br> Şifreniz: En az bir büyük-küçük harf ve sayı içermelidir ve en az sekiz karakter uzunluğunda olmalıdır.";
                return View("Login");
            }
            TempData["Errors"] = "E-Posta Adresinize gönderilen posta ile hesabınızı onaylayabilirsiniz.";
            return Redirect("/giris");
        }
        [HttpGet] public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Redirect("/giris");
        }
        [HttpGet] public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            bool result = await _userService.ConfirmEmail(userId, token);
            if (result)
            {
                TempData["Errors"] = "Hesabınız Doğrulandı Giriş Yapabilirsiniz";
            }
            else
            {
                TempData["Errors"] = "Hesabınız Doğrulanırken Beklenmedik Bir Hata İle Karşılaştık Lütfen Tekrar Deneyin <br> Tekrar Denemenize Rağmen Olmuyorsa Bizimle İletişime Geçin";
            }
            return View("Login");
        }
        [HttpGet] public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost] public async Task<IActionResult> ForgotPassword(string email)
        {
            bool resetPswResult = await _userService.SendResetPasswordRequest(email);
            if (resetPswResult)
                TempData["Errors"] = "E-Posta Adresinize Gönderilen Maildeki Linke Giderek Şifrenizi Yenileyebilirsiniz";
            else
                TempData["Errors"] = "Girdiğiniz E-Posta Adresi ile Eşleşen Bir Kayıda Rastlamadık";
            return Redirect("/auth/forgotPassword");
        }
        [HttpGet] public IActionResult ResetPassword(string userId, string token)
        {
            var model = new ResetPasswordModel() { userId = userId, Token = token };
            if (model.userId == null || model.Token == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }

        [HttpPost] public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            await _userService.ResetPasswordAsync(model);
            return Redirect("/giris");
        }
    }
}
