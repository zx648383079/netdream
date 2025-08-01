using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.Document.Models;
using NetDream.Modules.Document.Repositories;
using NetDream.Modules.OpenPlatform;

namespace NetDream.Api.Controllers.Document
{
    [Route("open/doc/[controller]")]
    [ApiController]
    public class PageController(PageRepository repository) : JsonController
    {
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(IPageModel), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Index(int id)
        {
            var res = repository.GetRead(id);
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return Render(res.Result);
        }
    }
}
