using Microsoft.AspNetCore.Http;
using NetDream.Shared.Interfaces;
using System.Threading.Tasks;

namespace NetDream.Web.Areas.Chat.WebSockets
{
    public class WebSocketManagerMiddleware
    {
        private readonly WebSocketHandler _socketHandler;

        public WebSocketManagerMiddleware(WebSocketHandler webSocketHandler)
        {
            _socketHandler = webSocketHandler;
        }

        public async Task InvokeAsync(HttpContext context, IClientContext client)
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                await _socketHandler.ConnectAsync(context, client);
            }
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
        }
    }
}
