using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace NetDream.Areas.Chat.WebSockets
{
    public class WebSocketConnectionManager
    {
        private readonly ConcurrentDictionary<string, WebSocketItem> _sockets = new ConcurrentDictionary<string, WebSocketItem>();
        public WebSocketItem GetSocketById(string id)
        {
            return _sockets.FirstOrDefault(p => p.Key == id).Value;
        }

        public ConcurrentDictionary<string, WebSocketItem> GetAll()
        {
            return _sockets;
        }

        public string GetId(WebSocketItem socket)
        {
            return _sockets.FirstOrDefault(p => p.Value == socket).Key;
        }

        public string GetId(WebSocket socket)
        {
            return _sockets.FirstOrDefault(p => p.Value.Socket == socket).Key;
        }

        public void AddSocket(WebSocketItem socket)
        {
            string sId = CreateConnectionId();
            while (!_sockets.TryAdd(sId, socket))
            {
                sId = CreateConnectionId();
            }



        }

        public async Task RemoveSocket(string id)
        {
            try
            {
                WebSocketItem socket;

                _sockets.TryRemove(id, out socket);


                await socket.Socket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, null, CancellationToken.None);


            }
            catch (Exception)
            {

            }

        }

        private string CreateConnectionId()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
