using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.Forum.Forms;
using NetDream.Modules.Forum.Models;
using NetDream.Modules.Forum.Repositories;
using NetDream.Modules.OpenPlatform.Models;

namespace NetDream.Api.Controllers.Forum
{

    [Route("open/forum")]
    [ApiController]
    public class ThreadController(ThreadRepository repository) : JsonController
    {
        [HttpGet]
        [Route("[controller]")]
        [ProducesResponseType(typeof(PageResponse<ThreadListItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Index([FromQuery] ThreadQueryForm form)
        {
            return RenderPage(repository.GetList(form));
        }

        [HttpGet]
        [Route("[controller]/[action]")]
        [ProducesResponseType(typeof(PageResponse<ThreadListItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Post([FromQuery] PostQueryForm form)
        {
            return RenderPage(repository.PostList(form));
        }

        [HttpGet]
        [Route("[controller]/[action]")]
        [ProducesResponseType(typeof(ThreadModel), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Detail(int id)
        {
            var res = repository.GetFull(id);
            if (res.Succeeded)
            {
                return Render(res.Result);
            }
            return RenderFailure(res.Message);
        }
    }
}
