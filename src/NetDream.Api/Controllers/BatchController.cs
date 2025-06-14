using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Api.Models;
using NetDream.Modules.Auth.Repositories;
using NetDream.Modules.SEO.Repositories;
using NetDream.Modules.UserAccount.Repositories;

namespace NetDream.Api.Controllers
{
    [Route("open/open/[controller]")]
    [ApiController]
    public class BatchController(
        OptionRepository seo,
        UserRepository auth) : JsonController
    {
        [HttpPost]
        public IActionResult Index([FromBody] BatchForm form)
        {
            var res = new BatchResult();
            if (form.SeoConfigs is not null)
            {
                res.SeoConfigs = seo.GetOpenList();
            }
            if (form.AuthProfile is not null)
            {
                res.AuthProfile = auth.GetCurrentProfile();
            }
            return Render(res);
        }
    }
}
