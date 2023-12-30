using System.Net.WebSockets;
using System.Threading.Tasks;
using System;
using WsServer.Manager;
using WsServer.Handler;

namespace WsServer.Services
{
    public class TimeHandler : WebSocketHandler
    {
        private System.Threading.Timer _timer;
        public TimeHandler(WebSocketConnectionManager webSocketConnectionManager) : base(webSocketConnectionManager)
        {
            _timer = new System.Threading.Timer(SendTime, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
        }
        private void SendTime(object state)
        {
            // 获取当前时间并发送给所有连接的客户端
            var currentTime = DateTime.Now.ToString("HH:mm:ss");
            SendMessageToAllAsync($"Current Time: {currentTime}").Wait();
        }
        public override async Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
        {
            var message = System.Text.Encoding.UTF8.GetString(buffer, 0, result.Count);
            var response = $"Received at {DateTime.Now:HH:mm:ss}";

            await SendMessageAsync(socket, response);
        }
    }
}
