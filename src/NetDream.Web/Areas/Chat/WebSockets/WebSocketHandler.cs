using Microsoft.AspNetCore.Http;
using NetDream.Shared.Interfaces;
using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetDream.Web.Areas.Chat.WebSockets
{
    public abstract class WebSocketHandler
    {
        public WebSocketConnectionManager ConnectionManager { get; set; }

        public WebSocketHandler(WebSocketConnectionManager connectionManager)
        {
            ConnectionManager = connectionManager;
        }

        public virtual async Task OnConnected(WebSocketItem socket)
        {
            ConnectionManager.AddSocket(socket);
        }

        public virtual async Task OnDisconnected(WebSocketItem socket)
        {
            await ConnectionManager.RemoveSocket(ConnectionManager.GetId(socket));
        }

        public async Task SendMessageAsync(WebSocketItem socket, string message)
        {
            if (socket.Socket.State != WebSocketState.Open)
            {
                return;
            }

            await socket.Socket.SendAsync(buffer: new ArraySegment<byte>(array: Encoding.UTF8.GetBytes(message),
                                                                  offset: 0,
                                                                  count: message.Length),
                                   messageType: WebSocketMessageType.Text,
                                   endOfMessage: true,
                                   cancellationToken: CancellationToken.None);
        }

        public async Task SendMessageAsync(string socketId, string message)
        {
            try
            {
                await SendMessageAsync(ConnectionManager.GetSocketById(socketId), message);
            }
            catch (Exception)
            {

            }

        }

        public async Task SendMessageToAllAsync(string message)
        {
            foreach (var pair in ConnectionManager.GetAll())
            {
                if (pair.Value.Socket.State == WebSocketState.Open)
                {
                    await SendMessageAsync(pair.Value, message);
                }
            }
        }

        public abstract Task ReceiveAsync(WebSocketItem socket, WebSocketReceiveResult result, byte[] buffer);
    
        
        public async Task ConnectAsync(HttpContext context, IClientContext client)
        {
            if (!context.WebSockets.IsWebSocketRequest)
            {
                return;
            }

            var socket = await context.WebSockets.AcceptWebSocketAsync();
            if (client.UserId == 0)
            {
                await socket.CloseOutputAsync(WebSocketCloseStatus.ProtocolError, "auth error", CancellationToken.None);
                return;
            }
            var socketItem = new WebSocketItem(socket)
            {
                AuthType = ChatAuthType.User,
                AuthId = client.UserId
            };
            await OnConnected(socketItem);
            var buffer = new byte[1024 * 4];

            while (socket.State == WebSocketState.Open)
            {
                var result = await socket.ReceiveAsync(buffer: new ArraySegment<byte>(buffer),
                                                       cancellationToken: CancellationToken.None);

                switch (result.MessageType)
                {
                    case WebSocketMessageType.Text:
                        await ReceiveAsync(socketItem, result, buffer);
                        break;
                    case WebSocketMessageType.Binary:
                        break;
                    case WebSocketMessageType.Close:
                        await OnDisconnected(socketItem);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
