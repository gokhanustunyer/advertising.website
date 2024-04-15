using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using tatil.business.Abstract.Service;
using tatil.entity.EntityReferences.Advert;
using tatil.entity.EntityReferences.Advert.Edit;
using tatil.entity.EntityReferences.Statics;
using tatil.entity.Identity;
using tatil.entity.Statics;

namespace tatil.webui.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IFilterService _filterService;
        private readonly IUserService _userService;
        private readonly IAdvertService _advertService;
        private readonly ISeoService _seoService;
        private readonly IPageStringsService _pageStringsService;
        private readonly IFormService _formService;
        private readonly ISitemapService _sitemapService;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment;
        private readonly IMainPageService _mainPageService;
        public AdminController(ICategoryService categoryService,
                               IFilterService filterService,
                               IUserService userService,
                               IAdvertService advertService,
                               ISeoService seoService,
                               IPageStringsService pageStringsService,
                               IFormService formService,
                               ISitemapService sitemapService,
                               Microsoft.AspNetCore.Hosting.IHostingEnvironment environment,
                               IMainPageService mainPageService)
        {
            _categoryService = categoryService;
            _filterService = filterService;
            _userService = userService;
            _advertService = advertService;
            _seoService = seoService;
            _pageStringsService = pageStringsService;
            _formService = formService;
            _sitemapService = sitemapService;
            _environment = environment;
            _environment = environment;
            _mainPageService = mainPageService;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet] public IActionResult Categories()
        {
            return View(_categoryService.GetAll());
        }
        [HttpGet] public IActionResult CreateCategory()
        {
            return View();
        }
        [HttpPost] public async Task<IActionResult> CreateCategory(string Name)
        {
            await _categoryService.AddAsync(Name);
            return Redirect("/admin/categories");
        }
        [HttpGet] public async Task<IActionResult> EditCategory(string id)
        {
            var model = await _categoryService.GetByIdWithAdvertsAsync(id);
            return View(model);
        }
        [HttpPost] public async Task<IActionResult> EditCategory(string id, string Name)
        {
            await _categoryService.UpdateCategoryNameByIdAsync(id, Name);
            return Redirect($"/admin/editCategory/{id}");
        }
        [HttpPost] public async Task<IActionResult> RemoveAdvertFromCategory(string categoryId, string advertId)
        {
            await _categoryService.RemoveAdvertFromCategoryAsync(categoryId, advertId);
            return Redirect($"/admin/editCategory/{categoryId}");
        }
        [HttpPost] public async Task<IActionResult> DeleteCategory(string id)
        {
            await _categoryService.DeleteCategory(id);
            return Redirect("/admin");
        }
        [HttpGet] public IActionResult Filters()
        {
            return View(_filterService.GetAllFilterBoxes());
        }
        [HttpGet] public IActionResult CreateFilter()
        {
            return View();
        }
        [HttpPost] public async Task<IActionResult> CreateFilter(string FilterBoxTitle, List<string> FilterNames)
        {
            await _filterService.AddAsync(FilterBoxTitle, FilterNames);
            return Redirect("/admin/filters");
        }
        [HttpGet] public IActionResult AdvertRequests()
        {
            var model = _userService.GetAdvertWaiters();            
            return View(model);
        }
        [HttpPost] public async Task<IActionResult> ConfirmAdvertRequest(string Id)
        {
            await _userService.ConfirmAdvertRequestAsync(Id);
            return Redirect("/admin/advertRequests");
        }
        [HttpPost] public async Task<IActionResult> RejectAdvertRequest(string Id)
        {
            await _userService.RejectAdvertRequestAsync(Id);
            return Redirect("/admin/advertRequests");
        }
        [HttpGet] public IActionResult ConfirmedAccounts()
        {
            var model = _userService.GetAdvertConfirmeds();
            return View(model);
        }
        [HttpGet] public async Task<IActionResult> EditUser(string id)
        {
            var model = await _userService.GetUserForEdit(id);
            return View(model);
        }
        [HttpPost] public async Task<IActionResult> EditUser(AppUser user)
        {
            var model = await _userService.UpdateUser(user);
            return View(model);
        }
        [HttpGet] public async Task<IActionResult> EditAdvert(string id)
        {
            var model = await  _advertService.GetAdvertForEditAsync(id);
            ViewBag.Filterboxes = _filterService.GetAllFilterBoxes();
            ViewBag.Categories = _categoryService.GetAll();
            return View(model);
        }
        [HttpPost] public async Task<IActionResult> EditAdvert(AdvertEditModel model)
        {
            await _advertService.UpdateAdvertAsync(model);
            return Redirect($"/admin/editAdvert/{model.Id}");
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
        [HttpPost] public async Task<IActionResult> AddImage(string id, int index, IFormFileCollection postedFiles)
        {
            await _advertService.UploadAdvertImageAsync(id, postedFiles);
            return Ok(true);
        }
        [HttpPost] public async Task<IActionResult> GetFiltersByFilterBoxId(string filterBoxId)
        {
            var response = await _filterService.GetFiltersByFilterBoxId(filterBoxId);
            return Ok(response);
        }
        [HttpGet] public async Task<IActionResult> EditFilterBox(string id)
        {
            var model = await _filterService.GetFilterBoxForEdit(id);
            return View(model);
        }
        [HttpPost] public async Task<IActionResult> EditFilterBox(string Id, string FilterBoxTitle)
        {
            await _filterService.UpdateFilterBox(Id, FilterBoxTitle);
            return Redirect($"/admin/editFilterBox/{Id}");
        }
        [HttpPost] public async Task<IActionResult> UpdateFilter(string filterName, string filterId)
        {
            var response = await _filterService.UpdateFilterName(filterId, filterName);
            return Ok(response);
        }
        [HttpPost] public async Task<IActionResult> AddNewFilterToFilterBox(string filterBoxId, string filterName)
        {
            var response = await _filterService.AddNewFilterToFilterBox(filterBoxId, filterName);
            return Ok(response);
        }
        [HttpPost] public async Task<IActionResult> DeleteFilterFromFilterBox(string filterBoxId, string filterId)
        {
            var response = await _filterService.DeleteFilterFromFilterBox(filterBoxId, filterId);
            return Ok(response);
        }
        [HttpGet] public async Task<IActionResult> FilterAdverts(string id)
        {
            var model = await _filterService.GetFilterDetailsModelById(id);
            return View(model);
        }
        [HttpPost] public async Task<IActionResult> RemoveAdvertFromFilter(string productId, string filterId)
        {
            await _filterService.RemoveProductFromFilter(filterId, productId);
            return Redirect($"/admin/filterAdverts/{filterId}");
        }
        [HttpPost] public async Task<IActionResult> DeleteFilterBox(string Id)
        {
            await _filterService.DeleteFilterBox(Id);
            return Redirect("/admin/filters");
        }
        [HttpPost] public async Task<IActionResult> DeleteAdvert(string id)
        {
            await _advertService.DeleteAdvert(id);
            return Redirect("/admin");
        }
        [HttpGet] public IActionResult SEOOptions()
        {
            var model = _seoService.GetAll();
            return View(model);
        }
        [HttpGet] public IActionResult NewSEO()
        {
            return View();
        }
        [HttpPost] public async Task<IActionResult> NewSEO(SEO seo)
        {
            await _seoService.Create(seo);
            return Redirect("/admin/seooptions");
        }
        [HttpGet] public IActionResult UpdateSEO(string id)
        {
            var model = _seoService.GetById(id);
            return View(model);
        }
        [HttpPost] public async Task<IActionResult> UpdateSEO(SEO seo)
        {
            await _seoService.Update(seo);
            return Redirect("/admin/seooptions");
        }
        [HttpPost] public async Task<IActionResult> DeleteSEO(string id)
        {
            await _seoService.DeleteById(id);
            return Redirect("/admin/seooptions");
        }
        [HttpGet] public IActionResult Settings()
        {
            var model = _pageStringsService.GetSettings();
            return View(model);
        }
        [HttpPost] public async Task<IActionResult> Settings(PageStrings pageStrings)
        {
            await _pageStringsService.UpdateSettings(pageStrings);
            return Redirect("/admin/settings");
        }
        [HttpGet] public IActionResult SupportMails()
        {
            var model = _formService.GetAllContactForms();
            return View(model);
        }
        [HttpGet] public IActionResult Adverts(string Search)
        {
            var model = _advertService.GetAllForListing(Search);
            return View(model);
        }
        [HttpGet] public IActionResult Users()
        {
            var model = _userService.GetAllUsersAsync();
            return View(model);
        }
        [HttpPost] public IActionResult Rebuildsitemap()
        {
            _sitemapService.CreateSitemap();
            return Redirect("/admin/settings");
        }
        [HttpGet] public IActionResult FTPTest()
        {
            return View();
        }
        [HttpPost] public IActionResult FTPTest(IFormFile file)
        {
            var Upload = Path.Combine(_environment.WebRootPath, "img", file.FileName);
            file.CopyTo(new FileStream(Upload, FileMode.Create));
            return Redirect("/admin/FTPTest");
        }
        [HttpGet] public IActionResult MainPageSettings()
        {
            var model = _mainPageService.GetSettings();
            return View(model);
        }
        [HttpPost] public async Task<IActionResult> MainPageSettings(SaveMainPageSettingsModel model)
        {
            await _mainPageService.SaveMainPageSettings(model);
            return Redirect("/admin/mainpagesettings");
        }
        [HttpGet] public IActionResult NewLinkBox()
        {
            return View();
        }
        [HttpPost] public async Task<IActionResult> NewLinkBox(CreateLinkBoxModel model)
        {
            await _mainPageService.CreateLinkBoxAsync(model);
            return Redirect("/admin/mainpagesettings");
        }
        [HttpPost] public async Task<IActionResult> DeletePageContent(string id)
        {
            await _mainPageService.DeleteContentAsync(id);
            return Redirect("/admin/mainpagesettings");
        }
        [HttpPost] public async Task<IActionResult> IncreaseContentIndex(string id)
        {
            await _mainPageService.IncreateContentIndexAsync(id);
            return Redirect("/admin/mainpagesettings");
        }
        [HttpPost] public async Task<IActionResult> MinusContentIndex(string id)
        {
            await _mainPageService.MinusContentIndexAsync(id);
            return Redirect("/admin/mainpagesettings");
        }
        [HttpGet] public async Task<IActionResult> EditLinkboxContent(string id)
        {
            var model = await _mainPageService.GetContentForEditAsync(id);
            return View(model);
        }
        [HttpPost] public async Task<IActionResult> EditLinkboxContent(CreateLinkBoxModel model)
        {
            await _mainPageService.UpdateLinkBoxAsync(model);
            return Redirect($"/admin/editlinkboxcontent/{model.Id}");
        }
    }
}
