using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.Auth.Entities;
using NetDream.Modules.Auth.Forms;
using NetDream.Modules.Auth.Models;
using NetDream.Modules.Auth.Repositories;
using NetDream.Modules.OpenPlatform;
using NetDream.Modules.UserIdentity.Repositories;

namespace NetDream.Api.Controllers.Auth
{
    [Route("open/auth/admin/invite")]
    [Authorize(Roles = IdentityRepository.Administrator)]
    [ApiController]
    public class InviteBackendController(InviteRepository repository) : JsonController
    {
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(PageResponse<InviteCodeEntity>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Index([FromQuery] CodeQueryForm form)
        {
            return RenderPage(repository.CodeList(form));
        }


        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataOneResponse<string>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Save([FromBody] CodeGenerateForm data)
        {
            var res = repository.CodeCreate(data);
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return RenderData(res.Result);
        }

        [HttpDelete]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataOneResponse<bool>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Delete(int id)
        {
            repository.CodeRemove(id);
            return RenderData(true);
        }

        [HttpDelete]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataOneResponse<bool>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Clear()
        {
            repository.CodeClear();
            return RenderData(true);
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(PageResponse<InviteLogListItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Log([FromQuery] CodeQueryForm form)
        {
            return RenderPage(repository.LogList(form));
        }
    }
}
