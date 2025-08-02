using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.Blog.Forms;
using NetDream.Modules.Blog.Models;
using NetDream.Modules.Blog.Repositories;
using NetDream.Modules.OpenPlatform;
using NetDream.Modules.UserIdentity.Repositories;

namespace NetDream.Api.Controllers.Blog
{
    [Route("open/blog/admin/blog")]
    [Authorize(Roles = IdentityRepository.Administrator)]
    [ApiController]
    public class PostBackendController(BlogRepository repository) : JsonController
    {
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(PageResponse<BlogListItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Index([FromQuery] BlogQueryForm form)
        {
            return RenderPage(repository.ManageList(form));
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(PageResponse<BlogModel>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Detail(int id)
        {
            var res = repository.ManageGet(id);
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return Render(res.Result);
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
