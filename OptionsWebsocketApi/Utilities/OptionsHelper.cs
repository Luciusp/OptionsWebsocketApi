using OptionsWebsocketApi.Models.Polygon;

namespace OptionsWebsocketApi.Utilities
{
    public static class OptionsHelper
    {
        private static readonly Random _random = new();
        public static OptionEvent GenerateRandomOptionEvent()
        {
            var oe = new OptionEvent()
            {
                Symbol = GenerateRandomSymbol(4),
                ExchangeId = _random.Next(0, 10),
                Price = _random.NextSingle(),
                TradeSize = _random.Next(0, 1000),
                TradeConditions = new int[] { 1 },
                Timestamp = DateTimeOffset.Now.ToUnixTimeSeconds()
            };

            return oe;
        }

        private static string GenerateRandomSymbol(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[_random.Next(s.Length)]).ToArray());
        }
    }
}
