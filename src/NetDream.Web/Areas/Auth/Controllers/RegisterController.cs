using Microsoft.AspNetCore.Mvc;

namespace NetDream.Web.Areas.Auth.Controllers
{
    [Area("Auth")]
    public class RegisterController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
