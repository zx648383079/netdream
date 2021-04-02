using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Web.Areas.Chat.WebSockets
{
    public class ChatRoomHandler : WebSocketHandler
    {
        const string MESSAGE_PREFIX = "ws-message:";
        const string MESSAGE_SEPARATOR = ";";

        public ChatRoomHandler(WebSocketConnectionManager webSocketConnectionManager) : base(webSocketConnectionManager)
        {

        }

        public override async Task ReceiveAsync(WebSocketItem socket, WebSocketReceiveResult result, byte[] buffer)
        {
            string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
            await SendMessageToAllAsync(message);
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
            return EncdoeMessage(even, WsMessageType.JSON, JsonConvert.SerializeObject(data));
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
            msg.Event = data[start..data.IndexOf(MESSAGE_SEPARATOR, start)];
            return msg;
        }
    }

    class WsMessage
    {
        public WsMessageType Type { get; set; }

        public string Content { get; set; }

        public string Event { get; set; }
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
