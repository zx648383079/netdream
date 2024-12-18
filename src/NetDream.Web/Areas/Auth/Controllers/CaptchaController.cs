using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetDream.Modules.Auth.Forms;
using NetDream.Modules.Auth.Repositories;

namespace NetDream.Web.Areas.Auth.Controllers
{
    [Area("Auth")]
    public class CaptchaController(CaptchaRepository repository) : Controller
    {
        public IActionResult Index([Bind] CaptchaForm form)
        {
            using var image = repository.Generate(form);
            HttpContext.Session.SetString("", string.Empty);
            return new FileContentResult(image.AsSpan(), image.ContentType);
        }
    }
}
