using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Api.Models;
using NetDream.Modules.Auth.Repositories;
using NetDream.Modules.SEO.Repositories;
using System.Collections.Generic;

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
            var res = new Dictionary<string, object?>();
            if (form.SeoConfigs is not null)
            {
                res.Add("seo_configs", seo.GetOpenList());
            }
            if (form.AuthProfile is not null)
            {
                res.Add("auth_profile", auth.GetCurrentProfile());
            }
            return Render(res);
        }
    }
}
