using System.Text.Json.Serialization;

namespace OptionsWebsocketApi.Models.Polygon
{
    public class OptionEvent
    {
        [JsonPropertyName("sym")]
        public string? Symbol { get; set; }

        [JsonPropertyName("x")]
        public int ExchangeId { get; set; }

        [JsonPropertyName("p")]
        public float Price { get; set; }

        [JsonPropertyName("s")]
        public int? TradeSize { get; set; }

        [JsonPropertyName("c")]
        public int[]? TradeConditions { get; set; }

        [JsonPropertyName("t")]
        public long Timestamp { get; set; }

    }
}
