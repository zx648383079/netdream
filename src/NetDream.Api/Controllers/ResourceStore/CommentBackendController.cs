using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.Comment.Entities;
using NetDream.Modules.Comment.Forms;
using NetDream.Modules.Comment.Models;
using NetDream.Modules.Comment.Repositories;
using NetDream.Modules.OpenPlatform;
using NetDream.Modules.UserIdentity.Repositories;
using NetDream.Shared.Models;

namespace NetDream.Api.Controllers.ResourceStore
{
    [Route("open/res/admin/comment")]
    [Authorize(Roles = IdentityRepository.Administrator)]
    [ApiController]
    public class CommentBackendController(CommentRepository repository) : JsonController
    {
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(PageResponse<CommentListItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Index([FromQuery] CommentQueryForm form)
        {
            return RenderPage(repository.AdvancedList(ModuleTargetType.Article, form));
        }


        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(CommentEntity), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Toggle(int id)
        {
            var res = repository.AdvancedToggle(id);
            if (res.Succeeded)
            {
                return Render(res.Result);
            }
            return RenderFailure(res.Message);
        }

        [HttpDelete]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataOneResponse<bool>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Delete(int id)
        {
            repository.AdvancedRemove(ModuleTargetType.Article, id);
            return RenderData(true);
        }
    }
}
