using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetDream.Web.Base.Http;

namespace NetDream.Web.Areas.Auth.Controllers.Api
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
