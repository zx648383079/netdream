using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.OnlineDisk.Forms;
using NetDream.Modules.OnlineDisk.Models;
using NetDream.Modules.OnlineDisk.Repositories;
using NetDream.Modules.OpenPlatform;

namespace NetDream.Api.Controllers.OnlineDisk
{
    [Route("open/disk/[controller]")]
    [ApiController]
    public class UploadController(DiskRepository repository) : JsonController
    {

        [HttpPost]
        [Route("")]
        [ProducesResponseType(typeof(DiskListItem), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Index([FromBody] DiskFileForm form)
        {
            var res = repository.Driver.UploadFile(form);
            if (res.Succeeded)
            {
                return Render(res.Result);
            }
            return RenderFailure(res.Message);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(DiskChunkResult), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Chunk([FromBody] DiskChunkForm form)
        {
            var res = repository.Driver.UploadChunk(form);
            if (res.Succeeded)
            {
                return Render(res.Result);
            }
            return RenderFailure(res.Message);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(DiskListItem), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Check([FromBody] DiskCheckForm form)
        {
            var res = repository.Driver.UploadCheck(form);
            if (res.Succeeded)
            {
                return Render(res.Result);
            }
            return RenderFailure(res.Message);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(DiskListItem), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Finish([FromBody] DiskChunkFinishForm form)
        {
            var res = repository.Driver.UploadFinish(form);
            if (res.Succeeded)
            {
                return Render(res.Result);
            }
            return RenderFailure(res.Message);
        }
    }
}
