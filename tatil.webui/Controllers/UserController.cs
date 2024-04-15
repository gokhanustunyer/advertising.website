using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using tatil.business.Abstract.Service;
using tatil.entity.EntityReferences.Advert;
using tatil.entity.EntityReferences.Advert.Create;
using tatil.entity.EntityReferences.Advert.Edit;
using tatil.entity.EntityReferences.User;
using tatil.entity.Identity;

namespace tatil.webui.Controllers
{
    public class UserController : Controller
    {
        private readonly IAdvertService _advertService;
        private readonly IUserService _userService;
        private readonly IFilterService _filterService;
        private readonly ICategoryService _categoryService;
        private readonly ICityDbService _cityDbService;
        public UserController(IUserService userService,
                              IAdvertService advertService,
                              ICityDbService cityDbService,
                              ICategoryService categoryService,
                              IFilterService filterService)
        {
            _userService = userService;
            _advertService = advertService;
            _cityDbService = cityDbService;
            _categoryService = categoryService;
            _filterService = filterService;
        }

        public async Task<IActionResult> Index()
        {
            if (!User.Identity.IsAuthenticated) { return Redirect("/giris"); }
            return View();
        }
        [HttpGet] public async Task<IActionResult> SendAdvertRequest()
        {
            if (!User.Identity.IsAuthenticated) { return Redirect("/giris"); }
            return View();
        }
        [HttpPost] public async Task<IActionResult> SendAdvertRequest(string AdvertRequestMessage, string email)
        {
            string username = User.Identity.Name;
            await _userService.SendAdvertRequestAsync(username, AdvertRequestMessage);
            return Redirect("/");
        }
        [HttpGet] public async Task<IActionResult> NewAdvert()
        {
            bool isConfirmedForAdverts = await _userService.IsConfirmedForAdvert(User.Identity.Name);
            if (!isConfirmedForAdverts) { return Redirect("/"); }
            var model = _advertService.GetAllForCreate();
            return View(model);
        }
        [HttpPost] public async Task<IActionResult> NewAdvert(CreateAdvertReference createAdvertReference)
        {
            if (!ModelState.IsValid)
            {
                var model = _advertService.GetAllForCreate();
                return View(model);
            }
            bool isConfirmedForAdverts = await _userService.IsConfirmedForAdvert(User.Identity.Name);
            if (!isConfirmedForAdverts) { return Redirect("/"); }
            createAdvertReference.userName = User.Identity.Name;
            await _advertService.CreateAdvert(createAdvertReference);
            return Redirect("/user/adverts");
        }
        [HttpPost] public IActionResult GetAllCities()
        {
            var model = _cityDbService.GetAllCities();
            return Ok(model);
        }
        [HttpPost] public IActionResult GetDistrict(int id)
        {
            return Ok(_cityDbService.GetAllDistrictByCityId(id));
        }
        [HttpPost] public IActionResult GetNeighborhood(int id)
        {
            return Ok(_cityDbService.GetAllNeighborhoodsByDistrict(id));
        }
        [HttpGet] public async Task<IActionResult> Adverts()
        {
            bool isConfirmedForAdverts = await _userService.IsConfirmedForAdvert(User.Identity.Name);
            if (!isConfirmedForAdverts) { return Redirect("/giris"); }
            var model = await _advertService.GetAdvertsByPublisher(User.Identity.Name);
            return View(model);
        }
        [HttpGet] public async Task<IActionResult> EditAdvert(string id)
        {
            bool isConfirmedForAdverts = await _userService.IsConfirmedForAdvert(User.Identity.Name);
            if (!isConfirmedForAdverts) { return Redirect("/giris"); }
            var model = await _advertService.GetAdvertForEditAsync(id);
            ViewBag.Filterboxes = _filterService.GetAllFilterBoxes();
            ViewBag.Categories = _categoryService.GetAll();
            return View(model);
        }
        [HttpPost] public async Task<IActionResult> EditAdvert(AdvertEditModel model)
        {
            if (!ModelState.IsValid)
            {
                var rmodel = await _advertService.GetAdvertForEditAsync(model.Id);
                ViewBag.Filterboxes = _filterService.GetAllFilterBoxes();
                ViewBag.Categories = _categoryService.GetAll();
                return View(rmodel);
            }
            await _advertService.UpdateAdvertAsync(model);
            return Redirect($"/user/editAdvert/{model.Id}");
        }
        [HttpGet] public async Task<IActionResult> Profile()
        {
            if (!User.Identity.IsAuthenticated) { return Redirect("/giris"); }
            var model = await _userService.GetUserProfile(User.Identity.Name);
            return View(model);
        }
        [HttpGet] public async Task<IActionResult> EditUserInfo()
        {
            if (!User.Identity.IsAuthenticated) { return Redirect("/giris"); }
            var model = await _userService.GetUserProfile(User.Identity.Name);
            return View(model);
        }
        [HttpPost] public async Task<IActionResult> EditUserInfo([Required(ErrorMessage = "İsim Alanını Boş Geçemezsiniz"), MinLength(2, ErrorMessage = "Geçersiz Uzunlukta İsim")] string FirstName, [Required(ErrorMessage = "Soyisim Alanını Boş Geçemezsiniz"), MinLength(2, ErrorMessage = "Geçersiz Uzunlukta Soyisim")] string LastName)
        {
            if (!ModelState.IsValid)
            {
                var modela = await _userService.GetUserProfile(User.Identity.Name);
                return View(modela);
            }
            var model = await _userService.UpdateUserNames(User.Identity.Name, FirstName, LastName);
            return Redirect("/user/profile");
        }
        [HttpGet] public async Task<IActionResult> UpdateEmail()
        {
            if (!User.Identity.IsAuthenticated) { return Redirect("/giris"); }
            AppUser user = await _userService.GetUserProfile(User.Identity.Name);
            return View(user);
        }

        [HttpPost] public async Task<IActionResult> UpdateEmail(UpdateEmailModel model)
        {
            if (!ModelState.IsValid)
            {
                AppUser user = await _userService.GetUserProfile(User.Identity.Name);
                return View(user);
            }
            model.UserName = User.Identity.Name;
            bool isComplete = await _userService.UpdateEmailAsync(model);
            if (!isComplete)
            {
                TempData["Erros"] = "Kayıt sırasında bir hata oluştur, seçitiğiniz e-posta adresi zaten kullanımda olabilir";
                AppUser user = await _userService.GetUserProfile(User.Identity.Name);
                return View(user);
            }
            return Redirect("/user/profile");
        }
        [HttpGet] public async Task<IActionResult> ConfirmUpdateEmail(string userId, string token, string newEmail)
        {
            var response = await _userService.ConfirmUpdateEmail(userId, token, newEmail);
            return Redirect("/user/profile");
        }
        [HttpPost] public async Task<IActionResult> UpdatePasswordRequest()
        {
            await _userService.SendUpdatePasswordRequestAsync(User.Identity.Name);
            return Ok(200);
        }
        [HttpGet] public async Task<IActionResult> UpdatePassword([FromQuery] string token)
        {
            if (!User.Identity.IsAuthenticated) { return Redirect("/giris"); }
            AppUser user = await _userService.GetUserProfile(User.Identity.Name);
            ViewData["PasswordResetToken"] = token;
            return View(user);
        }
        [HttpPost] public async Task<IActionResult> UpdatePassword(UpdatePasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                AppUser user = await _userService.GetUserProfile(User.Identity.Name);
                ViewData["PasswordResetToken"] = model.Token;
                return View(user);
            }
            model.UserName = User.Identity.Name;
            await _userService.UpdatePassword(model);
            return Redirect("/user/profile");
        }
        [HttpPost] public async Task<IActionResult> GetAdvertImages(string id)
        {
            var model = await _advertService.GetAdvertImageAsync(id);
            return Ok(model);
        }
        [HttpPost] public async Task<IActionResult> SaveImageAlignment([FromForm] string alignmentJson, string advertId)
        {
            var model = JsonConvert.DeserializeObject<List<ImageAndIndexsModel>>(alignmentJson);
            var response = await _advertService.ReIndex(model, advertId);
            return Ok();
        }
        [HttpPost] public async Task<IActionResult> DeleteImage(string ImageId, string AdvertId)
        {
            await _advertService.DeleteAdvertImage(ImageId, AdvertId);
            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> AddImage(string id, int index, IFormFileCollection postedFiles)
        {
            await _advertService.UploadAdvertImageAsync(id, postedFiles);
            return Ok(true);
        }
    }
}
