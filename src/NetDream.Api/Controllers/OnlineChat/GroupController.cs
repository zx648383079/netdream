using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.Chat.Entities;
using NetDream.Modules.Chat.Forms;
using NetDream.Modules.Chat.Models;
using NetDream.Modules.Chat.Repositories;
using NetDream.Modules.OpenPlatform;
using NetDream.Shared.Models;

namespace NetDream.Api.Controllers.OnlineChat
{
    [Route("open/chat/[controller]")]
    [Authorize]
    [ApiController]
    public class GroupController(GroupRepository repository) : JsonController
    {
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(DataResponse<GroupListItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Index()
        {
            return RenderData(repository.All());
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(PageResponse<GroupListItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Search([FromQuery] QueryForm form)
        {
            return RenderPage(repository.Search(form));
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(GroupModel), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Detail(int id)
        {
            var res = repository.Detail(id);
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return Render(res.Result);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataOneResponse<bool>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Agree([FromBody] ApplyForm data)
        {
            var res = repository.Agree(data);
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return RenderData(true);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataOneResponse<bool>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Apply([FromBody] ApplyForm data)
        {
            var res = repository.Apply(data);
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return RenderData(true);
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(PageResponse<ApplyListItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult ApplyLog([FromQuery] QueryForm form, int group)
        {
            return RenderPage(repository.ApplyLog(group, form));
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(GroupEntity), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Create([FromBody] GroupForm data)
        {
            var res = repository.Create(data);
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return Render(res.Result);
        }

        [HttpDelete]
        [Route("[action]")]
        [Authorize]
        [ProducesResponseType(typeof(DataOneResponse<bool>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Disband(int id)
        {
            var res = repository.Disband(id);
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return RenderData(true);
        }
    }
}
