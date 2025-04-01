using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.Auth.Models;
using NetDream.Modules.Auth.Repositories;
using NetDream.Modules.OpenPlatform.Models;

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
    }
}
