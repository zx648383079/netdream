using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetDream.Base.Http;

namespace NetDream.Areas.Auth.Controllers.Api
{
    public class HomeController : JsonController
    {
        public HomeController()
        {
        }

        [Route("open/auth")]
        public IActionResult Index()
        {
            return Json(JsonResponse.RenderData(true));
        }
    }
}
