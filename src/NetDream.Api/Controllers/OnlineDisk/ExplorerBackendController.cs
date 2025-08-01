using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.OpenPlatform;
using NetDream.Modules.UserIdentity.Repositories;
using NetDream.Shared.Providers.Entities;
using NetDream.Shared.Providers.Forms;
using NetDream.Shared.Repositories;
using NetDream.Shared.Repositories.Forms;
using NetDream.Shared.Repositories.Models;

namespace NetDream.Api.Controllers.OnlineDisk
{
    [Route("open/disk/admin/explorer")]
    [Authorize(Roles = IdentityRepository.Administrator)]
    [ApiController]
    public class ExplorerBackendController(ExplorerRepository repository) : JsonController
    {

        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(PageResponse<VirtualFileItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Index([FromQuery] ExplorerQueryForm form)
        {
            return RenderPage(repository.Search(form));
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataResponse<VirtualFileItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Drive()
        {
            return RenderData(repository.DriveList());
        }

        [HttpDelete]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataOneResponse<bool>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Delete(string path)
        {
            repository.Delete(path);
            return RenderData(true);
        }


        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(PageResponse<FileEntity>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Storage([FromQuery] StorageQueryForm form)
        {
            return RenderPage(repository.StorageSearch(form));
        }

        [HttpDelete]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataOneResponse<bool>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult StorageDelete(int[] id)
        {
            repository.StorageRemove(id);
            return RenderData(true);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataOneResponse<bool>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult StorageReload(int folder)
        {
            repository.StorageReload(folder);
            return RenderData(true);
        }
        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataOneResponse<bool>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult StorageSync(int[] id)
        {
            repository.StorageSync(id);
            return RenderData(true);
        }
    }
}
