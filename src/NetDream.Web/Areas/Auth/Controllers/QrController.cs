using Microsoft.AspNetCore.Mvc;

namespace NetDream.Web.Areas.Auth.Controllers
{
    [Area("Auth")]
    public class QrController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
