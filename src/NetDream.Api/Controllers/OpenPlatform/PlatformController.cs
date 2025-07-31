using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.OpenPlatform.Entities;
using NetDream.Modules.OpenPlatform.Forms;
using NetDream.Modules.OpenPlatform.Models;
using NetDream.Modules.OpenPlatform.Repositories;
using NetDream.Shared.Models;

namespace NetDream.Api.Controllers.OpenPlatform
{
    [Route("open/[controller]")]
    [Authorize]
    [ApiController]
    public class PlatformController(PlatformRepository repository) : JsonController
    {
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(PageResponse<PlatformListItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Index([FromQuery] QueryForm form)
        {
            return RenderPage(repository.SelfList(form));
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(PageResponse<PlatformEntity>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Detail(int id)
        {
            var res = repository.SelfGet(id);
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
            var res = repository.SelfSave(form);
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
            var res = repository.SelfRemove(id);
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return RenderData(true);
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataResponse<ListLabelItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult All()
        {
            return RenderData(repository.SelfAll());
        }
    }
}
