using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.WebSockets;
using System.Text;

namespace OptionsWebsocketApi.Controllers
{
    [ApiController]
    [Route("ws")]
    public class Socket : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ILogger<Socket> _logger;
        public Socket(IConfiguration config, ILogger<Socket> logger)
        {
            _config = config;
            _logger = logger;
        }
        [HttpGet]
        [Route("")]
        public async Task Get()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using WebSocket webSocket = await
                                   HttpContext.WebSockets.AcceptWebSocketAsync();
                await RelayOptions(HttpContext, webSocket);
            }
            else
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
        }

        private static async Task RelayOptions(HttpContext httpContext, WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];

            var numOptions = 10;
            for (int i = 0; i < numOptions; i++)
            {
                var msg = $"this is message {i}";
                var msgInBytes = Encoding.ASCII.GetBytes(msg);
                await webSocket.SendAsync(new ArraySegment<byte>(msgInBytes, 0, msgInBytes.Length), WebSocketMessageType.Text, true, CancellationToken.None);
            }
            
        }

        private static async Task Echo(HttpContext httpContext, WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            while (!result.CloseStatus.HasValue)
            {
                await webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), result.MessageType, result.EndOfMessage, CancellationToken.None);

                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }
            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }
    }
}
