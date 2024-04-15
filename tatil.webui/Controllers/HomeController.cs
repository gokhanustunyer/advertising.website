using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Diagnostics;
using System.Text.RegularExpressions;
using tatil.business.Abstract.Service;
using tatil.business.Concrete.Storage.Local;
using tatil.entity.PageEntities;
using tatil.webui.Models;

namespace tatil.webui.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAdvertService _advertService;
        private readonly ILocalStorage _localStorage;
        private readonly IPageStringsService _pageStringsService;
        private readonly IFormService _formService;
        private readonly ISitemapService _sitemapService;
        public HomeController(ILogger<HomeController> logger,
                              IAdvertService advertService,
                              ILocalStorage localStorage,
                              IPageStringsService pageStringsService,
                              IFormService formService,
                              ISitemapService sitemapService)
        {
            _logger = logger;
            _advertService = advertService;
            _localStorage = localStorage;
            _pageStringsService = pageStringsService;
            _formService = formService;
            _sitemapService = sitemapService;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _advertService.GetLastAdminAdvrets(9);
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpGet] public async Task<IActionResult> Adverts(string city, string district, string neighborhood, int bedCapacity, int humanCapacity, string? categoryUrl, string? search, List<string>? filters, int page = 1, string sort = "current")
        {
            ViewData["Category"] = (categoryUrl == null) ? "tatil-evleri" : categoryUrl;
            var model = await _advertService.GetAdvertsForListing(city, district, neighborhood, bedCapacity, humanCapacity, categoryUrl, search, filters, page, 12, sort);
            return View(model);
        }
        [HttpGet] public async Task<IActionResult> AdvertDetails(string advertUrl)
        {
            var model = await _advertService.GetAdvertDetailsByUrlAsync(advertUrl);
            return View(model);
        }
        [HttpPost] public async Task<IActionResult> UploadImage(IFormFileCollection image_param)
        {
            List<(string, string)> response = await _localStorage.UploadAsync("img", image_param);
            Hashtable fileUrl = new Hashtable();
            fileUrl.Add("link", response[0].Item2);
            return Json(fileUrl);
        }
        [HttpGet] public IActionResult AboutUs()
        {
            var model = _pageStringsService.GetSettings();
            return View(model);
        }
        [HttpPost] public async Task<IActionResult> SendContactMail(SupportFormModel formModel)
        {
            if (!ModelState.IsValid)
            {
                var model = _pageStringsService.GetSettings();
                return View("AboutUs", model);
            }
            await _formService.SaveContactForm(formModel);           
            return Redirect("/hakkimizda");
        }
        [HttpGet] [Route("sitemap.xml")] public IActionResult Sitemap()
        {
            return View();
        }
        [HttpGet] public IActionResult Contact()
        {
            return View();
        }
    }
}