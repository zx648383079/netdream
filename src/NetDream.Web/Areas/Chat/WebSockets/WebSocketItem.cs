using System.Collections.Generic;
using System.Net.WebSockets;

namespace NetDream.Web.Areas.Chat.WebSockets
{
    public class WebSocketItem
    {

        public WebSocket Socket { get; private set; }

        public ChatAuthType AuthType { get; set; }
        public int AuthId { get; set; }

        public ChatRoomType RoomType { get; set; }
        public int RoomId { get; set; }

        public WebSocketItem(WebSocket socket)
        {
            Socket = socket;
        }
    }

    public enum ChatAuthType
    {
        Guest,
        User
    }

    public enum ChatRoomType
    {
        UserToUser,
        UserToGroup,
        UserToService
    }
}
