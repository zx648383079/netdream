using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.OpenPlatform.Models;
using NetDream.Modules.ResourceStore.Entities;
using NetDream.Modules.ResourceStore.Models;
using NetDream.Modules.ResourceStore.Repositories;
using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Api.Controllers.ResourceStore
{
    [Route("open/res/[controller]")]
    [ApiController]
    public class CategoryController(CategoryRepository repository) : JsonController
    {
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(DataResponse<CategoryEntity>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Index(int parent = 0)
        {
            return RenderData(repository.GetChildren(parent));
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataResponse<CategoryModel>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Detail(int id)
        {
            var res = repository.GetFull(id);
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
        public IActionResult Tree(int parent = 0)
        {
            return RenderData(repository.Tree());
        }
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataResponse<CategoryTreeItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Level()
        {
            return RenderData(repository.LevelTree([]));
        }
    }
}
