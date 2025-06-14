using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.OpenPlatform.Models;
using NetDream.Modules.UserAccount.Models;
using NetDream.Modules.UserAccount.Repositories;

namespace NetDream.Api.Controllers.Auth
{
    [Route("open/auth/[controller]")]
    [ApiController]
    public class UserController(UserRepository auth) : JsonController
    {
        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(UserModel), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Index(string extra = "")
        {
            return Render(auth.GetCurrentProfile(extra));
        }

        [Route("[action]")]
        [HttpGet]
        [Authorize]
        public IActionResult Statistics()
        {
            return RenderData(auth.Statistics());
        }
    }
}
