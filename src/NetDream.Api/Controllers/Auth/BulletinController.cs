using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.Auth.Models;
using NetDream.Modules.Auth.Repositories;
using NetDream.Modules.OpenPlatform.Models;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;

namespace NetDream.Api.Controllers.Auth
{
    [Route("open/auth/[controller]")]
    [ApiController]
    public class BulletinController(BulletinRepository repository) : JsonController
    {
        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(IPage<BulletinUserListItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Index([FromQuery] QueryForm form, int status = 0,
            int user = 0,
            int lastId = 0)
        {
            return RenderPage(repository.GetList(form, status, user, lastId));
        }
    }
}
