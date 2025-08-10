using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.OpenPlatform;

namespace NetDream.Api.Controllers.Auth
{
    [Route("open/auth/[controller]")]
    [ApiController]
    public class QrController : JsonController
    {

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataOneResponse<bool>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Refresh()
        {

        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataOneResponse<bool>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Check(string token)
        {

        }

        [HttpPost]
        [Route("[action]")]
        [Authorize]
        [ProducesResponseType(typeof(DataOneResponse<bool>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Authorize(string token)
        {

        }
    }
}
