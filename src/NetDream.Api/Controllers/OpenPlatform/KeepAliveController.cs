using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.OpenPlatform;
using NetDream.Shared.Interfaces;

namespace NetDream.Api.Controllers.OpenPlatform
{
    [Route("open/[controller]")]
    [ApiController]
    public class KeepAliveController(IKeepAliveService service, IClientContext client) : JsonController
    {
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(DataOneResponse<object>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Index(string session_token = "")
        {
            if (service.TryGet(client.UserId, session_token, client.Ip, out var context))
            {
                return RenderData(context.Pop());
            }
            return RenderFailure("无效");
        }
    }
}
