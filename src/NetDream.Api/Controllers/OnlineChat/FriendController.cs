using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.Chat.Forms;
using NetDream.Modules.Chat.Models;
using NetDream.Modules.Chat.Repositories;
using NetDream.Modules.OpenPlatform;
using NetDream.Shared.Interfaces.Entities;
using NetDream.Shared.Models;

namespace NetDream.Api.Controllers.OnlineChat
{
    [Route("open/chat/[controller]")]
    [Authorize]
    [ApiController]
    public class FriendController(FriendRepository repository) : JsonController
    {
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(DataResponse<FriendGroupListItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Index()
        {
            return RenderData(repository.All());
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(PageResponse<IUserSource>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Search([FromQuery] QueryForm form)
        {
            return RenderPage(repository.Search(form));
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataResponse<FriendGroupListItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Classify()
        {
            return RenderData(repository.ClassifyList());
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataOneResponse<bool>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Agree([FromBody] ApplyForm data)
        {
            var res = repository.Follow(data);
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
            var res = repository.Follow(data);
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
        public IActionResult ApplyLog([FromQuery] QueryForm form)
        {
            return RenderPage(repository.ApplyLog(form));
        }

        [HttpDelete]
        [Route("[action]")]
        [Authorize]
        [ProducesResponseType(typeof(DataOneResponse<bool>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Delete(int id)
        {
            repository.Remove(id);
            return RenderData(true);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataOneResponse<bool>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Move(int user, int group)
        {
            var res = repository.Move(user, group);
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return RenderData(true);
        }
    }
}
