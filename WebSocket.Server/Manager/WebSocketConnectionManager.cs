using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;
using System.Threading;

namespace WsServer.Manager
{
    public class WebSocketConnectionManager
    {
        private readonly ConcurrentDictionary<string, WebSocket> _sockets = new ConcurrentDictionary<string, WebSocket>();
        public WebSocket GetSocketById(string id)
        {
            _sockets.TryGetValue(id, out var socket);
            return socket;
        }

        public ConcurrentDictionary<string, WebSocket> GetAll()
        {
            return _sockets;
        }

        public string GetId(WebSocket socket)
        {
            return _sockets.FirstOrDefault(x => x.Value == socket).Key;
        }

        public void AddSocket(WebSocket socket)
        {
            var id = Guid.NewGuid().ToString();
            _sockets.TryAdd(id, socket);
        }

        public async Task RemoveSocketAsync(string id)
        {
            _sockets.TryRemove(id, out var socket);
            await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by the WebSocketManager", CancellationToken.None);
        }
    }
}
