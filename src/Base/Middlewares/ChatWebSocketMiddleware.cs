using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetDream.Base.Middlewares
{
    public class ChatWebSocketMiddleware
    {
        private static ConcurrentDictionary<string, WebSocket> _sockets = new ConcurrentDictionary<string, WebSocket>();

        private readonly RequestDelegate _next;

        /// <summary>
        /// 缓冲区大小
        /// </summary>
        private const int bufferSize = 1024 * 4;

        public ChatWebSocketMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!IsWebSocket(context))
            {
                await _next.Invoke(context);
                return;
            }

            CancellationToken ct = context.RequestAborted;
            var currentSocket = await context.WebSockets.AcceptWebSocketAsync();
            //string socketId = Guid.NewGuid().ToString();
            string socketId = context.Request.Query["sid"].ToString();
            var auth = await context.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (auth.Succeeded)
            {
                var userId = auth.Principal.Identity.Name;
            }
            if (!_sockets.ContainsKey(socketId))
            {
                _sockets.TryAdd(socketId, currentSocket);
            }
            //_sockets.TryRemove(socketId, out dummy);
            //_sockets.TryAdd(socketId, currentSocket);

            while (true)
            {
                if (ct.IsCancellationRequested)
                {
                    break;
                }

                string response = await ReceiveStringAsync(currentSocket, ct);
                // MsgTemplate msg = JsonConvert.DeserializeObject<MsgTemplate>(response);

                if (string.IsNullOrEmpty(response))
                {
                    if (currentSocket.State != WebSocketState.Open)
                    {
                        break;
                    }
                    continue;
                }

                foreach (var socket in _sockets)
                {
                    if (socket.Value.State != WebSocketState.Open)
                    {
                        continue;
                    }
                    //if (socket.Key == msg.ReceiverID || socket.Key == socketId)
                    //{
                    //    await SendStringAsync(socket.Value, JsonConvert.SerializeObject(msg), ct);
                    //}
                }
            }

            //_sockets.TryRemove(socketId, out dummy);

            await currentSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", ct);
            currentSocket.Dispose();
        }

        private bool IsWebSocket(HttpContext context)
        {
            return context.WebSockets.IsWebSocketRequest &&
                context.Request.Path == "/Chat/Home/Ws";
        }

        private static Task SendStringAsync(WebSocket socket, string data, CancellationToken ct = default)
        {
            var buffer = Encoding.UTF8.GetBytes(data);
            var segment = new ArraySegment<byte>(buffer);
            return socket.SendAsync(segment, WebSocketMessageType.Text, true, ct);
        }

        private static async Task<string> ReceiveStringAsync(WebSocket socket, CancellationToken ct = default)
        {
            var buffer = new ArraySegment<byte>(new byte[bufferSize]);
            var result = await socket.ReceiveAsync(buffer, CancellationToken.None);
            while (!result.EndOfMessage)
            {
                result = await socket.ReceiveAsync(buffer, ct);
            }
            //if (result.MessageType != WebSocketMessageType.Text)
            //{
            //    return null;
            //}
            return Encoding.UTF8.GetString(buffer.Array);
        }
    }
}
