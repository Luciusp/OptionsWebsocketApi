using Microsoft.AspNetCore.SignalR;
using OptionsWebsocketApi.Models.Polygon;

namespace OptionsWebsocketApi.Hubs
{
    public class OptionsHub : Hub
    {
        private readonly Random _random = new();
        public async Task SendMessage()
        {
            while (true)
            {
                await Clients.All.SendAsync("ReceiveOption", GenerateRandomOptionEvent());
                Thread.Sleep(_random.Next(0,2)*_random.Next(100,1000));
            }
            
        }

        private OptionEvent GenerateRandomOptionEvent()
        {
            var oe = new OptionEvent()
            {
                Symbol = GenerateRandomSymbol(4),
                ExchangeId = _random.Next(0, 10),
                Price = _random.NextSingle(),
                TradeSize = _random.Next(0, 1000),
                TradeConditions = new int[] {1},
                Timestamp = DateTimeOffset.Now.ToUnixTimeSeconds()
            };

            return oe;
        }

        private string GenerateRandomSymbol(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[_random.Next(s.Length)]).ToArray());
        }
    }
}
