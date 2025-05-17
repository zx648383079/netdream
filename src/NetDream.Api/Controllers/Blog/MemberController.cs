using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.Blog.Repositories;
using NetDream.Shared.Models;

namespace NetDream.Api.Controllers.Blog
{
    [Route("open/blog")]
    [ApiController]
    public class MemberController(BlogRepository repository) : JsonController
    {
        [Route("publish/page")]
        [Authorize]
        public IActionResult Index([FromQuery] QueryForm form)
        {
            return RenderPage(repository.GetList(form));
        }

        [Route("home/edit_option")]
        [Authorize]
        public IActionResult Option()
        {
            return Render(null);
        }
    }
}
