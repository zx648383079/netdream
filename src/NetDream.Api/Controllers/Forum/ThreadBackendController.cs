using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.Forum.Forms;
using NetDream.Modules.Forum.Models;
using NetDream.Modules.Forum.Repositories;
using NetDream.Modules.OpenPlatform;
using NetDream.Modules.UserIdentity.Repositories;

namespace NetDream.Api.Controllers.Forum
{
    [Route("open/forum/admin/thread")]
    [Authorize(Roles = IdentityRepository.Administrator)]
    [ApiController]
    public class ThreadBackendController(ThreadRepository repository) : JsonController
    {
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(PageResponse<ThreadListItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Index([FromQuery] ThreadQueryForm form)
        {
            return RenderPage(repository.ManageList(form));
        }

        [HttpDelete]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataOneResponse<bool>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Delete(int id)
        {
            repository.ManageRemove(id);
            return RenderData(true);
        }
    }
}
