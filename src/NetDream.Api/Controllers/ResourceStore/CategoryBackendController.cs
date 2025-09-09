using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.UserIdentity.Repositories;
using NetDream.Modules.ResourceStore.Repositories;
using NetDream.Modules.ResourceStore.Models;
using NetDream.Modules.ResourceStore.Forms;
using NetDream.Modules.ResourceStore.Entities;
using NetDream.Shared.Models;
using NetDream.Modules.OpenPlatform;

namespace NetDream.Api.Controllers.ResourceStore
{
    [Route("open/res/admin/category")]
    [Authorize(Roles = IdentityRepository.Administrator)]
    [ApiController]
    public class CategoryBackendController(CategoryRepository repository) : JsonController
    {
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(DataResponse<CategoryTreeItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Index()
        {
            return RenderData(repository.LevelTree([]));
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(CategoryEntity), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Save([FromBody] CategoryForm form)
        {
            var res = repository.Save(form);
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
            repository.Remove(id);
            return RenderData(true);
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataResponse<CategoryTreeItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult All()
        {
            return RenderData(repository.LevelTree([]));
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(PageResponse<CategoryEntity>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Search(QueryBoundForm form)
        {
            return RenderPage(repository.Search(form, form.Id));
        }
    }
}
