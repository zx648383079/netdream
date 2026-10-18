using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NetDream.Web.Areas.Chat.WebSockets
{
    public class ChatRoomHandler(WebSocketConnectionManager connectionManager) 
        : WebSocketHandler(connectionManager)
    {
        const string MESSAGE_PREFIX = "ws-message:";
        const string MESSAGE_SEPARATOR = ";";


        public override async Task ReceiveAsync(WebSocketItem socket, WebSocketReceiveResult result, byte[] buffer)
        {
            var message = DecdoeMessage(Encoding.UTF8.GetString(buffer, 0, result.Count));
            switch (message.Event)
            {
                case "chat/message":

                    break;
                case "chat/message/ping":
                    break;
                case "chat/message/send_text":
                    break;
                default:
                    break;
            }
            // await SendMessageToAllAsync(message);
        }

        private static string EncdoeMessage(string even, string data)
        {
            return EncdoeMessage(even, WsMessageType.STRING, data);
        }

        private static string EncdoeMessage(string even, int data)
        {
            return EncdoeMessage(even, WsMessageType.INT, data.ToString());
        }

        private static string EncdoeMessage(string even, bool data)
        {
            return EncdoeMessage(even, WsMessageType.BOOL, data.ToString());
        }

        private static string EncdoeMessage(string even, object data)
        {
            return EncdoeMessage(even, WsMessageType.JSON, JsonSerializer.Serialize(data));
        }

        private static string EncdoeMessage(string even, WsMessageType type, string data)
        {
            return $"{MESSAGE_PREFIX}{even}{MESSAGE_SEPARATOR}{type}{MESSAGE_SEPARATOR}{data}";
        }

        private static WsMessage DecdoeMessage(string data)
        {
            if (!data.StartsWith(MESSAGE_PREFIX))
            {
                return new WsMessage()
                {
                    Type = WsMessageType.UNKNOW,
                    Content = data
                };
            }
            var msg = new WsMessage();
            var start = MESSAGE_PREFIX.Length;
            msg.Event = data[start..data.IndexOf(MESSAGE_SEPARATOR, start)];
            start += MESSAGE_SEPARATOR.Length + msg.Event.Length;
            msg.Type = (WsMessageType)int.Parse(data[start..data.IndexOf(MESSAGE_SEPARATOR, start)]);
            start += MESSAGE_SEPARATOR.Length + msg.Type.ToString().Length;
            msg.Content = data[start..data.IndexOf(MESSAGE_SEPARATOR, start)];
            return msg;
        }
    }

    class WsMessage
    {
        public WsMessageType Type { get; set; }

        public string Content { get; set; } = string.Empty;

        public string Event { get; set; } = string.Empty;
    }

    enum WsMessageType
    {
        STRING,
        INT,
        BOOL,
        JSON = 4,
        UNKNOW = 99,
    }
}
