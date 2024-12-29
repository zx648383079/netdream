using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.SEO.Forms;

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
