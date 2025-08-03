using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.Chat.Repositories;
using NetDream.Modules.Note.Forms;
using NetDream.Modules.Note.Models;
using NetDream.Modules.OpenPlatform;

namespace NetDream.Api.Controllers.OnlineChat
{
    [Route("open/chat/[controller]")]
    [Authorize]
    [ApiController]
    public class UserController(ChatRepository repository) : JsonController
    {

        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Index()
        {
            return Render(repository.GetProfile());
        }
    }
}
