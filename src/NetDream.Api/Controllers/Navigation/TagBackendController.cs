using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.OpenPlatform;
using NetDream.Modules.Tag.Entities;
using NetDream.Modules.Tag.Repositories;
using NetDream.Modules.UserIdentity.Repositories;
using NetDream.Shared.Models;

namespace NetDream.Api.Controllers.Navigation
{
    [Route("open/navigation/admin/tag")]
    [Authorize(Roles = IdentityRepository.Administrator)]
    [ApiController]
    public class TagBackendController(TagRepository repository) : JsonController
    {
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(PageResponse<TagEntity>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Index([FromQuery] QueryForm form)
        {
            return RenderPage(repository.AdvancedList(ModuleTargetType.SearchSite, form));
        }

        [HttpDelete]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataOneResponse<bool>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Delete(int id)
        {
            repository.AdvancedRemove(ModuleTargetType.SearchSite, id);
            return RenderData(true);
        }
    }
}
