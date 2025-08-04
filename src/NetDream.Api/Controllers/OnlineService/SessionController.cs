using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.OnlineService.Entities;
using NetDream.Modules.OnlineService.Forms;
using NetDream.Modules.OnlineService.Models;
using NetDream.Modules.OnlineService.Repositories;
using NetDream.Modules.OpenPlatform;
using NetDream.Modules.UserIdentity.Repositories;

namespace NetDream.Api.Controllers.OnlineService
{
    [Route("open/os/admin/[controller]")]
    [Authorize(Roles = IdentityRepository.Administrator)]
    [ApiController]
    public class SessionController(SessionRepository repository) : JsonController
    {
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(PageResponse<SessionListItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Index([FromQuery] SessionQueryForm form)
        {
            return RenderPage(repository.GetList(form));
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataResponse<SessionListItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult My([FromQuery] SessionQueryForm form)
        {
            return RenderData(repository.MyList());
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(SessionEntity), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Remark(int session_id, string remark)
        {
            var res = repository.Remark(session_id, remark);
            if (res.Succeeded)
            {
                return Render(res.Result);
            }
            return RenderFailure(res.Message);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(SessionEntity), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Transfer(int session_id, int user)
        {
            var res = repository.Transfer(session_id, user);
            if (res.Succeeded)
            {
                return Render(res.Result);
            }
            return RenderFailure(res.Message);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(SessionEntity), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Reply(int session_id, int word)
        {
            var res = repository.Reply(session_id, word);
            if (res.Succeeded)
            {
                return Render(res.Result);
            }
            return RenderFailure(res.Message);
        }

    }
}
