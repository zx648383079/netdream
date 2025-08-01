using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.OnlineDisk.Forms;
using NetDream.Modules.OnlineDisk.Repositories;
using NetDream.Modules.OpenPlatform;
using NetDream.Modules.UserIdentity.Repositories;
using NetDream.Shared.Models;
using FileListItem = NetDream.Modules.OnlineDisk.Models.FileListItem;

namespace NetDream.Api.Controllers.OnlineDisk
{
    [Route("open/disk/admin/file")]
    [Authorize(Roles = IdentityRepository.Administrator)]
    [ApiController]
    public class FileBackendController(ServerRepository repository) : JsonController
    {
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(PageResponse<FileListItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Index([FromQuery] ServerFileQueryForm form)
        {
            return RenderPage(repository.FileList(form));
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(PageResponse<FileListItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Server([FromQuery] QueryForm form)
        {
            return RenderPage(repository.ServerList(form));
        }
    }
}
