using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.OpenPlatform;
using NetDream.Modules.UserAccount.Models;
using NetDream.Modules.UserAccount.Repositories;

namespace NetDream.Api.Controllers.Auth
{
    [Route("open/auth/[controller]")]
    [ApiController]
    public class SpaceController(SpaceRepository repository) : JsonController
    {
        [HttpGet]
        [ProducesResponseType(typeof(UserProfile), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Index(int user)
        {
            var res = repository.Get(user);
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return Render(res.Result);
        }

        [HttpPost]
        [Route("[action]")]
        [Authorize]
        [ProducesResponseType(typeof(DataOneResponse<int>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Follow(int user)
        {
            var res = repository.ToggleFollow(user);
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return RenderData(res.Result);
        }

        [HttpPost]
        [Route("[action]")]
        [Authorize]
        [ProducesResponseType(typeof(DataOneResponse<int>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Mark(int user)
        {
            var res = repository.ToggleMark(user);
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return RenderData(res.Result);
        }

        [HttpPost]
        [Route("[action]")]
        [Authorize]
        [ProducesResponseType(typeof(DataOneResponse<bool>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Report(int user, string reason)
        {
            var res = repository.Report(user, reason);
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return RenderData(true);
        }
    }
}
