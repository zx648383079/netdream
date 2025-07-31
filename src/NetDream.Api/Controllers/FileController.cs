using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.OpenPlatform.Models;
using NetDream.Shared.Models;
using NetDream.Shared.Repositories;

namespace NetDream.Api.Controllers
{
    [Route("open/[controller]")]
    [Authorize]
    [ApiController]
    public class FileController(FileRepository repository) : JsonController
    {
        [HttpPost]
        [Route("")]
        [ProducesResponseType(typeof(DataResponse<FileUploadResult>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Index(IFormFile file)
        {
            var res = repository.UploadFiles(new FormUploadFile(file));
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return Render(res.Result);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataResponse<FileUploadResult>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Base64(IFormFile file)
        {
            var res = repository.UploadBase64(new FormUploadFile(file));
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return Render(res.Result);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataResponse<FileUploadResult>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Image(IFormFile file)
        {
            var res = repository.UploadImages(new FormUploadFile(file));
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return Render(res.Result);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataResponse<FileUploadResult>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Video(IFormFile file)
        {
            var res = repository.UploadVideo(new FormUploadFile(file));
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return Render(res.Result);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataResponse<FileUploadResult>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Audio(IFormFile file)
        {
            var res = repository.UploadAudio(new FormUploadFile(file));
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return Render(res.Result);
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(PageResponse<FileListItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Images([FromQuery] QueryForm form)
        {
            return RenderPage(repository.ImageList(form));
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(PageResponse<FileListItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Files([FromQuery] QueryForm form)
        {
            return RenderPage(repository.FileList(form));
        }
    }
}
