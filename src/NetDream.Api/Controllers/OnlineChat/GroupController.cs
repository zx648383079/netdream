using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.Chat.Forms;
using NetDream.Modules.OpenPlatform;
using NetDream.Modules.Team.Entities;
using NetDream.Modules.Team.Forms;
using NetDream.Modules.Team.Models;
using NetDream.Modules.Team.Repositories;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;

namespace NetDream.Api.Controllers.OnlineChat
{
    [Route("open/team")]
    [Authorize]
    [ApiController]
    public class GroupController(TeamRepository repository) : JsonController
    {
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(DataResponse<TeamListItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Index()
        {
            return RenderData(repository.All());
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(PageResponse<TeamListItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Search([FromQuery] QueryForm form)
        {
            return RenderPage(repository.Search(form));
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(TeamModel), 200)]
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
            var res = repository.Agree(data.User, data.Group);
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
            var res = repository.Apply(data.User, data.Group, data.Remark);
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return RenderData(true);
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(PageResponse<IApplyListItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult ApplyLog([FromQuery] QueryForm form, int group)
        {
            return RenderPage(repository.ApplyLog(group, form));
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(TeamEntity), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Create([FromBody] TeamForm data)
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
