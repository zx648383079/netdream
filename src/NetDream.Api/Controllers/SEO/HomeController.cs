using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetDream.Modules.SEO.Repositories;
using NetDream.Web.Base.Http;

namespace NetDream.Api.Controllers.SEO
{
    [Route("open/seo")]
    [ApiController]
    public class HomeController(OptionRepository repository) : JsonController
    {

        public IActionResult Index()
        {
            return Render(repository.GetOpenList());
        }
    }
}
