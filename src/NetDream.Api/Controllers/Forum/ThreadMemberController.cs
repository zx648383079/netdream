using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.Forum.Entities;
using NetDream.Modules.Forum.Forms;
using NetDream.Modules.Forum.Models;
using NetDream.Modules.Forum.Repositories;
using NetDream.Modules.OpenPlatform;
using NetDream.Shared.Models;

namespace NetDream.Api.Controllers.Forum
{
    [Route("open/forum/member")]
    [Authorize]
    [ApiController]
    public class ThreadMemberController(ThreadRepository repository) : JsonController
    {
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(PageResponse<ThreadListItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Thread([FromQuery] ThreadQueryForm form)
        {
            return RenderPage(repository.SelfList(form));
        }

        [HttpDelete]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataOneResponse<bool>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult ThreadDelete(int id)
        {
            var res = repository.Remove(id);
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return RenderData(true);
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(PageResponse<ThreadPostEntity>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Post([FromQuery] QueryForm form)
        {
            return RenderPage(repository.SelfPostList(form));
        }

        [HttpDelete]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataOneResponse<bool>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult PostDelete(int id)
        {
            var res = repository.RemovePost(id);
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return RenderData(true);
        }
    }
}
