using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.OpenPlatform;
using NetDream.Modules.UserAccount.Models;
using NetDream.Modules.UserAccount.Repositories;
using NetDream.Shared.Models;

namespace NetDream.Api.Controllers.Auth
{
    [Route("open/auth/[controller]")]
    [ApiController]
    public class UserController(UserRepository auth) : JsonController
    {
        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(UserProfileModel), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Index(string extra = "")
        {
            return Render(auth.GetCurrentProfile(extra));
        }

        [Route("[action]")]
        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(DataResponse<StatisticsItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Statistics()
        {
            return RenderData(auth.Statistics());
        }


    }
}
