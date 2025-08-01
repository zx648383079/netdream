using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.OnlineDisk.Models;
using NetDream.Modules.OnlineDisk.Repositories;
using NetDream.Modules.OpenPlatform;
using NetDream.Shared.Models;

namespace NetDream.Api.Controllers.OnlineDisk
{
    [Route("open/disk/[controller]")]
    [ApiController]
    public class ShareController(ShareRepository repository) : JsonController
    {
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(PageResponse<ShareListItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Index([FromQuery] QueryForm form)
        {
            return RenderPage(repository.PublicList(form));
        }

        [HttpGet]
        [Route("[action]")]
        [Authorize]
        [ProducesResponseType(typeof(PageResponse<ShareListItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult My([FromQuery] QueryForm form)
        {
            return RenderPage(repository.MyList(form));
        }

        [HttpDelete]
        [Route("[action]")]
        [Authorize]
        [ProducesResponseType(typeof(DataOneResponse<bool>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Delete(int id)
        {
            repository.Remove(id);
            return RenderData(true);
        }
    }
}
