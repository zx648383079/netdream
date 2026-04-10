using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.OpenPlatform;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;

namespace NetDream.Api.Controllers
{
    [Route("open/[controller]")]
    [Authorize]
    [ApiController]
    public class FileController(IStorageRepository repository, IClientContext client) : JsonController
    {
        [HttpPost]
        [Route("")]
        [ProducesResponseType(typeof(DataResponse<IFileListItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Index(IFormFile file)
        {
            var res = repository.UploadFile(client.UserId, new FormUploadFile(file));
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return Render(res.Result);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataResponse<IFileListItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Base64(string file)
        {
            var res = repository.UploadBase64(client.UserId, file);
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return Render(res.Result);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataResponse<IFileListItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Image(IFormFile file)
        {
            var res = repository.UploadImage(client.UserId, new FormUploadFile(file));
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return Render(res.Result);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataResponse<IFileListItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Video(IFormFile file)
        {
            var res = repository.UploadVideo(client.UserId, new FormUploadFile(file));
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return Render(res.Result);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataResponse<IFileListItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Audio(IFormFile file)
        {
            var res = repository.UploadAudio(client.UserId, new FormUploadFile(file));
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return Render(res.Result);
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(PageResponse<IFileListItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Images([FromQuery] QueryForm form)
        {
            return RenderPage(repository.SearchImages(form));
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(PageResponse<IFileListItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Files([FromQuery] QueryForm form)
        {
            return RenderPage(repository.Search(form));
        }
    }
}
