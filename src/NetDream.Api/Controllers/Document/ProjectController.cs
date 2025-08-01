using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.Document.Entities;
using NetDream.Modules.Document.Forms;
using NetDream.Modules.Document.Models;
using NetDream.Modules.Document.Repositories;
using NetDream.Modules.OpenPlatform;
using NetDream.Shared.Interfaces.Entities;
using NetDream.Shared.Models;

namespace NetDream.Api.Controllers.Document
{
    [Route("open/doc/[controller]")]
    [ApiController]
    public class ProjectController(ProjectRepository repository) : JsonController
    {
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(PageResponse<ProjectEntity>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Index([FromQuery] ProjectQueryForm form)
        {
            return RenderPage(repository.GetList(form));
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(ProjectEntity), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Detail(int id)
        {
            var res = repository.Get(id);
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return Render(res.Result);
        }
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataResponse<ListLabelItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Version(int id)
        {
            if (!repository.CanOpen(id))
            {
                return RenderFailure("无权限浏览");
            }
            return RenderData(repository.VersionAll(id));
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataResponse<ITreeItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Catalog(int id, int version = 0)
        {
            if (!repository.CanOpen(id))
            {
                return RenderFailure("无权限浏览");
            }
            return RenderData(repository.Catalog(id, version));
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataResponse<IPageModel>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Page(int project, int id)
        {
            if (!repository.CanOpen(project))
            {
                return RenderFailure("无权限浏览");
            }
            return Render(repository.Page(project, id));
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataResponse<ListLabelItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Suggest(string keywords)
        {
            return RenderData(repository.Suggest(keywords));
        }
    }
}
