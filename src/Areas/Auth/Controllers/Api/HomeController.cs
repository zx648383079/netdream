using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetDream.Base.Http;

namespace NetDream.Areas.Auth.Controllers.Api
{
    [Area("Auth/Api")]
    public class HomeController : JsonController
    {
        public HomeController(IHttpContextAccessor accessor): base(accessor)
        {
        }

        public IActionResult Index()
        {

            return View();
        }
    }
}
