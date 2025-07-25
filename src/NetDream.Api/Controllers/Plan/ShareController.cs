using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.OpenPlatform.Models;
using NetDream.Modules.Plan.Entities;
using NetDream.Modules.Plan.Forms;
using NetDream.Modules.Plan.Models;
using NetDream.Modules.Plan.Repositories;
using NetDream.Shared.Models;

namespace NetDream.Api.Controllers.Plan
{
    [Route("open/task/[controller]")]
    [Authorize()]
    [ApiController]
    public class ShareController(ShareRepository repository) : JsonController
    {
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(PageResponse<ShareListItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Index([FromQuery] QueryForm form)
        {
            return RenderPage(repository.GetList(form));
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(PageResponse<ShareListItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult My([FromQuery] QueryForm form)
        {
            return RenderPage(repository.MyList(form));
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(ShareModel), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Detail(int id)
        {
            var res = repository.Detail(id);
            if (res.Succeeded)
            {
                return Render(res.Result);
            }
            return RenderFailure(res.Message);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(ShareEntity), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Create([FromBody] ShareForm form)
        {
            var res = repository.Create(form);
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
            var res = repository.Remove(id);
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return RenderData(true);
        }

        [HttpDelete]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataOneResponse<bool>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult DeleteUser(int id, int user)
        {
            var res = repository.RemoveUser(id, user);
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return RenderData(true);
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(PageResponse<UserRoleItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Users(int id)
        {
            return RenderData(repository.Users(id));
        }
    }
}
