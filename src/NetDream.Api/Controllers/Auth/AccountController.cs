using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.Auth.Entities;
using NetDream.Modules.Auth.Models;
using NetDream.Modules.Auth.Repositories;
using NetDream.Modules.OpenPlatform;
using NetDream.Modules.UserAccount.Forms;
using NetDream.Modules.UserAccount.Models;
using NetDream.Modules.UserAccount.Repositories;

namespace NetDream.Api.Controllers.Auth
{
    [Route("open/auth/[controller]")]
    [Authorize]
    [ApiController]
    public class AccountController(
        AccountRepository repository,
        LogRepository logStore) : JsonController
    {
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(PageResponse<AccountLogListItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Log([FromQuery] LogQueryForm form)
        {
            return RenderPage(repository.SelfLogList(form));
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataResponse<ConnectListItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Connect()
        {
            return RenderData(logStore.SelfConnect());
        }

        [HttpDelete]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataOneResponse<bool>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult ConnectDelete(int id)
        {
            var res = logStore.SelfConnectRemove(id);
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return RenderData(true);
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataResponse<DriverListItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Driver()
        {
            return RenderData(logStore.SelfDriver());
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataResponse<AccountLogListItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Authorize()
        {
            return RenderData(logStore.SelfAuthorize());
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(PageResponse<LoginLogEntity>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult LoginLog([FromQuery] LogQueryForm form)
        {
            return RenderPage(logStore.SelfLoginLog(form));
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(UserSubtotalResult), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Subtotal()
        {
            return Render(repository.SelfSubtotal());
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataOneResponse<bool>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Cancel(string reason)
        {
            var res = repository.SelfCancel(reason);
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return RenderData(true);
        }
    }
}
