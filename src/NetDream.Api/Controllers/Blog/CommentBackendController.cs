using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.Blog.Entities;
using NetDream.Modules.Blog.Models;
using NetDream.Modules.Blog.Repositories;
using NetDream.Modules.OpenPlatform;
using NetDream.Modules.UserIdentity.Repositories;
using NetDream.Shared.Providers.Forms;

namespace NetDream.Api.Controllers.Blog
{
    [Route("open/blog/admin/[controller]")]
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
            return RenderPage(repository.ManageList(form));
        }


        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(CommentEntity), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Toggle(int id)
        {
            var res = repository.ManageToggle(id);
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
            repository.ManageRemove(id);
            return RenderData(true);
        }
    }
}
