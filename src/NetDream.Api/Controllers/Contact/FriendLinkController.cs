using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetDream.Modules.Auth.Models;
using NetDream.Modules.Contact.Entities;
using NetDream.Modules.Contact.Repositories;
using NetDream.Modules.OpenPlatform.Models;
using NetDream.Web.Base.Http;

namespace NetDream.Api.Controllers.Contact
{
    [Route("open/contact")]
    [ApiController]
    public class FriendLinkController(ContactRepository repository) : JsonController
    {

        [HttpGet]
        [Route("friend_link")]
        [ProducesResponseType(typeof(DataResponse<FriendLinkEntity>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Index()
        {
            return RenderData(repository.FriendLinks());
        }
    }
}
