using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.Article.Models;
using NetDream.Modules.OpenPlatform;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;

namespace NetDream.Api.Controllers.ResourceStore
{
    [Route("open/res/[controller]")]
    [ApiController]
    public class CategoryController(ICategoryRepository repository) : JsonController
    {
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(DataResponse<CategoryTreeItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Index(int parent = 0)
        {
            return RenderData(repository.Children(ModuleTargetType.ResourceStore, parent));
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataResponse<CategoryTreeItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Detail(int id)
        {
            var res = repository.Get(ModuleTargetType.ResourceStore, id, true);
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return Render(res.Result);
        }
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataResponse<ITreeItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Tree()
        {
            return RenderData(repository.Tree(ModuleTargetType.ResourceStore));
        }
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataResponse<ILevelItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Level()
        {
            return RenderData(repository.All(ModuleTargetType.ResourceStore));
        }
    }
}
