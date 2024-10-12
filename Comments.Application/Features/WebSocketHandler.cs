using Microsoft.AspNetCore.Http;
using System.Net.WebSockets;
using System.Text;

namespace Comments.Application.Features
{
    public class WebSocketHandler
    {
        private readonly RequestDelegate _next;
        private static readonly List<WebSocket> _sockets = new List<WebSocket>();

        public WebSocketHandler(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path == "/ws")
            {
                if (context.WebSockets.IsWebSocketRequest)
                {
                    var webSocket = await context.WebSockets.AcceptWebSocketAsync();
                    _sockets.Add(webSocket);
                    await HandleWebSocketAsync(webSocket);
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                }
            }
            else
            {
                await _next(context);
            }
        }

        private async Task HandleWebSocketAsync(WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];

            while (webSocket.State == WebSocketState.Open)
            {
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
                }
            }

            _sockets.Remove(webSocket);
        }

        public static async Task SendMessageToAll(string message)
        {
            var buffer = Encoding.UTF8.GetBytes(message);
            var segment = new ArraySegment<byte>(buffer);

            foreach (var socket in _sockets)
            {
                if (socket.State == WebSocketState.Open)
                {
                    await socket.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
        }
    }
}