using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.OnlineService.Forms;
using NetDream.Modules.OnlineService.Models;
using NetDream.Modules.OnlineService.Repositories;
using NetDream.Modules.OpenPlatform;
using NetDream.Modules.UserIdentity.Repositories;
using NetDream.Shared.Repositories;

namespace NetDream.Api.Controllers.OnlineService
{
    [Route("open/os")]
    [ApiController]
    public class HomeController(ChatRepository repository, FileRepository fileStore) : JsonController
    {

        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(MessageQueryResult), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Index([FromQuery] MessageQueryForm form)
        {
            return Render(repository.MessageList(form));
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(MessageQueryResult), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Send([FromBody] MessageQueryForm form, 
            IFormFile? file, int type = 0)
        {
            form.SessionId = repository.SessionId(form.SessionToken);
            MessageForm body;
            if (file is not null)
            {
                var r = fileStore.UploadImage(new FormUploadFile(file));
                if (!r.Succeeded)
                {
                    return RenderFailure(r.Message);
                }
                body = new MessageForm()
                {
                    Type = type > 0 ? (byte)type : ChatRepository.TYPE_IMAGE,
                    Content = r.Result.Url
                };
            } else
            {
                body = new MessageForm()
                {
                    Type = (byte)type,
                    Content = Request.Form.TryGetValue("content", out var r) ? r : string.Empty,
                };
            }
            var res = repository.Send(form.SessionId, body);
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return Render(repository.MessageList(form));
        }

        [HttpGet]
        [Route("admin/session/message")]
        [Authorize(Roles = IdentityRepository.Administrator)]
        [ProducesResponseType(typeof(MessageQueryResult), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult CSMessage([FromQuery] MessageQueryForm form)
        {
            if (repository.CSEnabled(form.SessionId))
            {
                return RenderFailure("此会话不属于你，无法查看");
            }
            return Render(repository.CSMessageList(form));
        }

        [HttpPost]
        [Route("admin/session/send")]
        [Authorize(Roles = IdentityRepository.Administrator)]
        [ProducesResponseType(typeof(MessageQueryResult), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult CSSend([FromBody] MessageQueryForm form,
            IFormFile? file, int type = 0)
        {
            MessageForm body;
            if (file is not null)
            {
                var r = fileStore.UploadImage(new FormUploadFile(file));
                if (!r.Succeeded)
                {
                    return RenderFailure(r.Message);
                }
                body = new MessageForm()
                {
                    Type = type > 0 ? (byte)type : ChatRepository.TYPE_IMAGE,
                    Content = r.Result.Url
                };
            }
            else
            {
                body = new MessageForm()
                {
                    Type = (byte)type,
                    Content = Request.Form.TryGetValue("content", out var r) ? r : string.Empty,
                };
            }
            var res = repository.Send(form.SessionId, body, 1);
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return Render(repository.CSMessageList(form));
        }
    }
}
