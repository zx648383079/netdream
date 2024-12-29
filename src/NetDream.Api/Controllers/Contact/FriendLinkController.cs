using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.Contact.Models;
using NetDream.Modules.Contact.Repositories;
using NetDream.Modules.OpenPlatform.Models;

namespace NetDream.Api.Controllers.Contact
{
    [Route("open/contact")]
    [ApiController]
    public class FriendLinkController(ContactRepository repository) : JsonController
    {

        [HttpGet]
        [Route("friend_link")]
        [ProducesResponseType(typeof(DataResponse<FriendLink>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Index()
        {
            return RenderData(repository.FriendLinks());
        }
    }
}
