using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.Article.Entities;
using NetDream.Modules.Article.Forms;
using NetDream.Modules.Article.Repositories;
using NetDream.Modules.OpenPlatform;
using NetDream.Modules.UserIdentity.Repositories;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;

namespace NetDream.Api.Controllers.ResourceStore
{
    [Route("open/res/admin/category")]
    [Authorize(Roles = IdentityRepository.Administrator)]
    [ApiController]
    public class CategoryBackendController(CategoryRepository repository) : JsonController
    {
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(DataResponse<ILevelItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Index()
        {
            return RenderData(repository.All(ModuleTargetType.ResourceStore));
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(CategoryEntity), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Save([FromBody] CategoryForm form)
        {
            var res = repository.AdvancedSave(ModuleTargetType.ResourceStore, form);
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
            repository.AdvancedRemove(ModuleTargetType.ResourceStore, id);
            return RenderData(true);
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataResponse<ILevelItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult All()
        {
            return RenderData(repository.All(ModuleTargetType.ResourceStore));
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(PageResponse<CategoryEntity>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Search(QueryBoundForm form)
        {
            return RenderPage(repository.Search(ModuleTargetType.ResourceStore, form, form.Id));
        }
    }
}
