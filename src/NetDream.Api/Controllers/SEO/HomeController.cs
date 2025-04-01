using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.SEO.Repositories;

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
