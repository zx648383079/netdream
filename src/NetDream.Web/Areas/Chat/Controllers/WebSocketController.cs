using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetDream.Shared.Interfaces;
using NetDream.Web.Areas.Chat.WebSockets;
using System.Threading.Tasks;

namespace NetDream.Web.Areas.Chat.Controllers
{
    [Area("Chat")]
    public class WebSocketController(WebSocketHandler socketHandler, IClientContext client) : ControllerBase
    {
        [Route("/ws")]
        public async Task Get()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                await socketHandler.ConnectAsync(HttpContext, client);
            }
            else
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
        }

    }
}
