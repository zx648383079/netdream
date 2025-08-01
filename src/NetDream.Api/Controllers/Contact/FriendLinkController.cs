using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.Contact.Entities;
using NetDream.Modules.Contact.Forms;
using NetDream.Modules.Contact.Models;
using NetDream.Modules.Contact.Repositories;
using NetDream.Modules.OpenPlatform;

namespace NetDream.Api.Controllers.Contact
{
    [Route("open/contact/friend_link")]
    [ApiController]
    public class FriendLinkController(ContactRepository repository) : JsonController
    {

        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(DataResponse<FriendLink>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Index()
        {
            return RenderData(repository.FriendLinks());
        }

        [HttpPost]
        [Route("[action]")]
        [Authorize]
        [ProducesResponseType(typeof(FriendLinkEntity), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Apply([FromBody] FriendLinkForm form)
        {
            var res = repository.ApplyFriendLink(form);
            if (res.Succeeded)
            {
                return Render(res.Result);
            }
            return RenderFailure(res.Message);
        }
    }
}
