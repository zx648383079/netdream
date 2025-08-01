using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.OpenPlatform;
using NetDream.Modules.ResourceStore.Repositories;
using NetDream.Modules.UserIdentity.Repositories;
using NetDream.Shared.Models;
using NetDream.Shared.Providers.Forms;
using NetDream.Shared.Providers.Models;

namespace NetDream.Api.Controllers.ResourceStore
{
    [Route("open/res/admin/comment")]
    [Authorize(Roles = IdentityRepository.Administrator)]
    [ApiController]
    public class CommentBackendController(ResourceRepository repository) : JsonController
    {
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(PageResponse<CommentListItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Index([FromQuery] CommentQueryForm form)
        {
            return RenderPage(repository.Comment().Search(form));
        }

        [HttpDelete]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataOneResponse<bool>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Delete(int id)
        {
            repository.Comment().Remove(id);
            return RenderData(true);
        }
    }
}
