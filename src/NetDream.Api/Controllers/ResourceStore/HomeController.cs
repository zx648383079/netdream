using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.OpenPlatform.Models;
using NetDream.Modules.ResourceStore.Forms;
using NetDream.Modules.ResourceStore.Models;
using NetDream.Modules.ResourceStore.Repositories;
using NetDream.Shared.Models;

namespace NetDream.Api.Controllers.ResourceStore
{
    [Route("open/res/[controller]")]
    [ApiController]
    public class HomeController(ResourceRepository repository) : JsonController
    {
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(PageResponse<ResourceListItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Index([FromQuery] ResourceQueryForm form)
        {
            return RenderPage(repository.GetList(form));
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(PageResponse<ResourceModel>), 200)]
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
        [ProducesResponseType(typeof(PageResponse<ResourceModel>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Preview(int id)
        {
            var res = repository.GetPreview(id);
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return Render(res.Result);
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(FileStreamResult), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Download(int id, int file = 0)
        {
            var res = repository.Download(id, file);
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return new FileStreamResult(res.Result.OpenRead(), res.Result.FileType)
            {
                FileDownloadName = res.Result.Name
            };
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataResponse<CatalogItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Catalog(int id)
        {
            return RenderData(repository.GetCatalog(id));
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataResponse<ListArticleItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Suggest(string keywords)
        {
            return RenderData(repository.Suggest(keywords));
        }
    }
}
