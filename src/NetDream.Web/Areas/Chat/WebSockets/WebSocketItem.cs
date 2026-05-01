using System.Collections.Generic;
using System.Net.WebSockets;

namespace NetDream.Web.Areas.Chat.WebSockets
{
    public class WebSocketItem
    {

        public WebSocket Socket { get; private set; }

        public Dictionary<string, object> Items { get; private set; } = [];

        public WebSocketItem(WebSocket socket)
        {
            Socket = socket;
        }
    }
}
