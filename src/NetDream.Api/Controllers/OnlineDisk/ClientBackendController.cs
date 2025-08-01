using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.OnlineDisk.Forms;
using NetDream.Modules.OnlineDisk.Repositories;
using NetDream.Modules.OpenPlatform;
using NetDream.Modules.UserIdentity.Repositories;

namespace NetDream.Api.Controllers.OnlineDisk
{
    [Route("open/disk/admin/client")]
    [Authorize(Roles = IdentityRepository.Administrator)]
    [ApiController]
    public class ClientBackendController(ClientRepository repository) : JsonController
    {



        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(ServerSourceForm), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult LinkSave([FromBody] ServerSourceForm form)
        {
            var res = repository.Link(form);
            if (res.Succeeded)
            {
                return Render(res.Result);
            }
            return RenderFailure(res.Message);
        }
    }
}
