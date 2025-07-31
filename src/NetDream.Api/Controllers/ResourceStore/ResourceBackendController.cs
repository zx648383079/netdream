using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.OpenPlatform.Models;
using NetDream.Modules.ResourceStore.Entities;
using NetDream.Modules.ResourceStore.Forms;
using NetDream.Modules.ResourceStore.Models;
using NetDream.Modules.ResourceStore.Repositories;
using NetDream.Modules.UserIdentity.Repositories;
using NetDream.Shared.Models;
using System.IO;

namespace NetDream.Api.Controllers.ResourceStore
{
    [Route("open/res/admin/home")]
    [Authorize(Roles = IdentityRepository.Administrator)]
    [ApiController]
    public class ResourceBackendController(ResourceRepository repository) : JsonController
    {
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(PageResponse<ResourceListItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Index([FromQuery] ResourceQueryForm form)
        {
            return RenderPage(repository.GetManageList(form));
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(ResourceEntity), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Save([FromBody] ResourceForm form)
        {
            var res = repository.Save(form);
            if (res.Succeeded)
            {
                return Render(res.Result);
            }
            return RenderFailure(res.Message);
        }

        [HttpDelete]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataOneResponse<bool>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Delete(int id)
        {
            repository.Remove(id);
            return RenderData(true);
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(PageResponse<ResourceFileEntity>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult File(FileQueryForm form)
        {
            return RenderPage(repository.FileList(form));
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(ResourceFileEntity), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult FileSave([FromBody] FileForm form)
        {
            var res = repository.FileSave(form);
            if (res.Succeeded)
            {
                return Render(res.Result);
            }
            return RenderFailure(res.Message);
        }

        [HttpDelete]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataOneResponse<bool>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult FileDelete(int id)
        {
            repository.FileRemove(id);
            return RenderData(true);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(FileUploadResult), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Upload(IFormFile file)
        {
            var res = repository.Upload(new FormUploadFile(file));
            if (res.Succeeded)
            {
                return Render(res.Result);
            }
            return RenderFailure(res.Message);
        }
    }
}
