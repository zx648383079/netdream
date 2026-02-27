using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.Forum.Models;
using NetDream.Modules.Forum.Repositories;
using NetDream.Modules.OpenPlatform;

namespace NetDream.Api.Controllers.Forum
{

    [Route("open/forum")]
    [ApiController]
    public class ForumController(ForumRepository repository) : JsonController
    {
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(DataResponse<ForumListItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Index(int parent_id = 0)
        {
            return RenderData(repository.Children(parent_id));
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(ForumModel), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Detail(int id, bool full = true)
        {
            var res = repository.GetFull(id, full);
            if (res.Succeeded)
            {
                return Render(res.Result);
            }
            return RenderFailure(res.Message);
        }

    }
}
