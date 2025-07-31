using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.OpenPlatform.Entities;
using NetDream.Modules.OpenPlatform.Forms;
using NetDream.Modules.OpenPlatform.Models;
using NetDream.Modules.OpenPlatform.Repositories;
using NetDream.Modules.UserIdentity.Repositories;

namespace NetDream.Api.Controllers.OpenPlatform
{
    [Route("open/admin/[controller]")]
    [Authorize(Roles = IdentityRepository.Administrator)]
    [ApiController]
    public class PlatformBackendController(PlatformRepository repository) : JsonController
    {

        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(PageResponse<PlatformListItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Index([FromQuery] PlatformQueryForm form)
        {
            return RenderPage(repository.GetList(form));
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(PageResponse<PlatformEntity>), 200)]
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

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(PlatformEntity), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Save([FromBody] PlatformForm form)
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
    }
}
