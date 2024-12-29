using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.Blog.Repositories;
using NetDream.Shared.Models;

namespace NetDream.Api.Controllers.Blog
{
    [Route("open/blog")]
    [ApiController]
    public class HomeController(BlogRepository repository) : JsonController
    {

        public IActionResult Index([FromQuery] QueryForm form)
        {
            return RenderPage(repository.GetList(form));
        }
    }
}
