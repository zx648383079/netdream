using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.Chat.Forms;
using NetDream.Modules.Chat.Models;
using NetDream.Modules.Chat.Repositories;
using NetDream.Modules.OpenPlatform;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;

namespace NetDream.Api.Controllers.OnlineChat
{
    [Route("open/chat/[controller]")]
    [Authorize]
    [ApiController]
    public class MessageController(MessageRepository repository) : JsonController
    {
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(MessageQueryResult), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Index([FromQuery] MessageQueryForm form)
        {
            return Render(repository.GetList(form));
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataResponse<HistoryListItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Ping([FromQuery] MessageQueryForm form)
        {
            return RenderData(repository.Ping(form));
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(MessageListItem), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult SendText(int type, int id, string content)
        {
            var res = repository.SendText(type, id, content);
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return Render(res.Result);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataResponse<MessageListItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult SendImage(int type, int id, IFormFileCollection file)
        {
            return RenderData(repository.SendImage(type, id, new FormUploadFileCollection(file)));
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(MessageListItem), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult SendVideo(int type, int id, IFormFile file)
        {
            var res = repository.SendVideo(type, id, new FormUploadFile(file));
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return Render(res.Result);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(MessageListItem), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult SendAudio(int type, int id, IFormFile file)
        {
            var res = repository.SendAudio(type, id, new FormUploadFile(file));
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return Render(res.Result);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(MessageListItem), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult SendFile(int type, int id, IFormFile file)
        {
            var res = repository.SendFile(type, id, new FormUploadFile(file));
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
        public IActionResult Revoke(int id)
        {
            var res = repository.Revoke(id);
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return RenderData(true);
        }
    }
}
