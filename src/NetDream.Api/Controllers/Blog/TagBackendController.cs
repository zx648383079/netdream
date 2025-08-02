using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.Blog.Repositories;
using NetDream.Modules.OpenPlatform;
using NetDream.Modules.UserIdentity.Repositories;
using NetDream.Shared.Models;
using NetDream.Shared.Providers.Entities;

namespace NetDream.Api.Controllers.Blog
{
    [Route("open/blog/admin/tag")]
    [Authorize(Roles = IdentityRepository.Administrator)]
    [ApiController]
    public class TagBackendController(BlogRepository repository) : JsonController
    {
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(DataResponse<TagEntity>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Index([FromQuery] QueryForm form)
        {
            return RenderData(repository.Tag().GetList(form));
        }

        [HttpDelete]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataOneResponse<bool>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Delete(int id)
        {
            repository.Tag().Remove(id);
            return RenderData(true);
        }
    }
}
