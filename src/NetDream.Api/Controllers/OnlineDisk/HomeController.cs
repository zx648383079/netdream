using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.OnlineDisk.Forms;
using NetDream.Modules.OnlineDisk.Models;
using NetDream.Modules.OnlineDisk.Repositories;
using NetDream.Modules.OpenPlatform;
using System.Linq;

namespace NetDream.Api.Controllers.OnlineDisk
{
    [Route("open/disk/[controller]")]
    [Authorize]
    [ApiController]
    public class HomeController(DiskRepository repository) : JsonController
    {
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(PageResponse<DiskListItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Index([FromQuery] CatalogQueryForm form)
        {
            return RenderPage(repository.Driver.Catalog(form));
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(PageResponse<DiskListItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Search([FromQuery] DiskQueryForm form)
        {
            return RenderPage(repository.Driver.Search(form));
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataOneResponse<DiskListItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Create([FromBody] DiskFolderForm data)
        {
            var res = repository.Driver.Create(data);
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return Render(res.Result);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataOneResponse<DiskListItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Rename([FromBody] DiskRenameForm data)
        {
            var res = repository.Driver.Rename(data);
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
        public IActionResult Delete(string id)
        {
            var res = repository.Driver.Remove(id);
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return RenderData(true);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataOneResponse<DiskListItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Allow(string url)
        {
            var res = repository.AllowUrl(url);
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return RenderData(res.Result);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataOneResponse<DiskListItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult File(string id)
        {
            var res = repository.File(id);
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return RenderData(res.Result);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataOneResponse<DiskListItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Files(string[] id)
        {
            var res = repository.File(id.FirstOrDefault());
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return RenderData(res.Result);
        }
    }
}
