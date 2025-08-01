using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.OpenPlatform;
using NetDream.Modules.ResourceStore.Repositories;
using NetDream.Modules.UserIdentity.Repositories;
using NetDream.Shared.Models;
using NetDream.Shared.Providers.Entities;

namespace NetDream.Api.Controllers.ResourceStore
{
    [Route("open/res/admin/tag")]
    [Authorize(Roles = IdentityRepository.Administrator)]
    [ApiController]
    public class TagBackendController(ResourceRepository repository) : JsonController
    {
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(PageResponse<TagEntity>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Index([FromQuery] QueryForm form)
        {
            return RenderPage(repository.Tag().GetList(form));
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

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataResponse<TagEntity>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult All()
        {
            return RenderData(repository.Tag().All());
        }
    }
}
