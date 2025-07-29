using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.Forum.Entities;
using NetDream.Modules.Forum.Forms;
using NetDream.Modules.OpenPlatform.Models;
using NetDream.Modules.UserIdentity.Repositories;

namespace NetDream.Api.Controllers.Navigation
{
    [Route("open/navigation/[controller]")]
    [Authorize(Roles = IdentityRepository.Administrator)]
    [ApiController]
    public class BatchController : JsonController
    {
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(PageResponse<ForumEntity>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Index([FromQuery] ForumQueryForm form)
        {
            return RenderPage(repository.GetList(form));
        }
    }
}
