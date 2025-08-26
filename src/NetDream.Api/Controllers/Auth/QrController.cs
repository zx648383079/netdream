using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.Auth.Forms;
using NetDream.Modules.Auth.Models;
using NetDream.Modules.Auth.Repositories;
using NetDream.Modules.OpenPlatform;
using NetDream.Modules.UserAccount.Models;

namespace NetDream.Api.Controllers.Auth
{
    [Route("open/auth/[controller]")]
    [ApiController]
    public class QrController(AuthRepository repository) : JsonController
    {

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(QrResult), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Refresh()
        {
            var res = repository.QrRefresh();
            if (!res.Succeeded)
            {
                return RenderFailure(res);
            }
            return Render(res.Result);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(UserProfileModel), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Check(string token)
        {
            var res = repository.QrCheck(token);
            if (!res.Succeeded)
            {
                return RenderFailure(res);
            }
            return Render(res.Result);
        }

        [HttpPost]
        [Route("[action]")]
        [Authorize]
        [ProducesResponseType(typeof(DataOneResponse<bool>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Authorize([FromBody] QrAuthorizeForm data)
        {
            var res = repository.QrAuthorize(data);
            if (!res.Succeeded)
            {
                return RenderFailure(res);
            }
            return RenderData(true);
        }
    }
}
