using Microsoft.AspNetCore.Mvc;
using tatil.business.Abstract.Service;
using tatil.entity.EntityReferences.Advert.Create;

namespace tatil.webui.Controllers
{
    public class BusinessController : Controller
    {
        private readonly IAdvertService _advertService;

        public BusinessController(IAdvertService advertService)
        {
            _advertService = advertService;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet] public IActionResult NewAdvert()
        {
            var model = _advertService.GetAllForCreate();
            return View(model);
        }
        [HttpPost] public async Task<IActionResult> NewAdvert(CreateAdvertReference createAdvertReference)
        {
            await _advertService.CreateAdvert(createAdvertReference);
            return Redirect("/business/newAdvert");
        }
    }
}
