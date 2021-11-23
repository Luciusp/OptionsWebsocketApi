using Microsoft.AspNetCore.Mvc;
using OptionsWebsocketApi.Models.Polygon;
using OptionsWebsocketApi.Utilities;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace OptionsWebsocketApi.Controllers
{
    [ApiController]
    [Route("ws")]
    public class Socket : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ILogger<Socket> _logger;
        private static readonly Random _random = new();
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

                int i = 0;
                while (true)
                {
                    
                    await RelayOptions(webSocket, i);
                    i++;
                    Thread.Sleep(_random.Next(0, 2) * _random.Next(100, 1000));
                }
            }
            else
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
        }

        private static async Task RelayOptions(WebSocket webSocket, int i)
        {
            var oeList = new List<OptionEvent>();
            for (int j = 0; j < _random.Next(1,10); j++)
            {
                var oe = OptionsHelper.GenerateRandomOptionEvent();
                oeList.Add(oe);
            }
            var msgInBytes = Encoding.ASCII.GetBytes(JsonSerializer.Serialize(oeList));
            await webSocket.SendAsync(new ArraySegment<byte>(msgInBytes, 0, msgInBytes.Length), WebSocketMessageType.Text, true, CancellationToken.None);

        }
    }
}
