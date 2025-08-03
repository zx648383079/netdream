using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.Chat.Models;
using NetDream.Modules.Chat.Repositories;
using NetDream.Modules.OpenPlatform;
using NetDream.Shared.Models;

namespace NetDream.Api.Controllers.OnlineChat
{
    [Route("open/chat/[controller]")]
    [Authorize]
    [ApiController]
    public class ChatController(ChatRepository repository) : JsonController
    {
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(PageResponse<HistoryListItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Index([FromQuery] QueryForm form)
        {
            return RenderPage(repository.Histories(form));
        }

        [HttpDelete]
        [Route("[action]")]
        [Authorize]
        [ProducesResponseType(typeof(DataOneResponse<bool>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Delete(int id)
        {
            repository.RemoveIdHistory(id);
            return RenderData(true);
        }
    }
}
