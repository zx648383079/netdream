using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetDream.Modules.SEO.Forms;
using NetDream.Web.Base.Http;

namespace NetDream.Api.Controllers.SEO
{
    [Route("open/seo/[controller]")]
    [ApiController]
    public class BatchController : JsonController
    {
        [HttpPost]
        public IActionResult Index([FromForm] BatchForm form)
        {
            return Render(null);
        }
    }
}
