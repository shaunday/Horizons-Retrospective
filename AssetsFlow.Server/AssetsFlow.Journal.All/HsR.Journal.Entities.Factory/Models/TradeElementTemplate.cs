using System.Text.Json.Serialization;

namespace HsR.Journal.Entities.Factory.Models
{
    public class TradeElementTemplate
    {
        [JsonPropertyName("elementType")]
        public TradeActionType ElementType { get; set; }

        [JsonPropertyName("elements")]
        public List<DataElementTemplate> Elements { get; set; } = new();
    }

    public class DataElementTemplate
    {
        [JsonPropertyName("title")]

        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("componentType")]
        public ComponentType ComponentType { get; set; }

        [JsonPropertyName("restrictions")]
        public List<string>? Restrictions { get; set; }

        [JsonPropertyName("firstPositionOnly")]
        public bool? FirstPositionOnly { get; set; }

        [JsonPropertyName("isRelevantForLocalOverview")]
        public bool? IsRelevantForLocalOverview { get; set; }

        [JsonPropertyName("isRelevantForTradeOverview")]
        public bool? IsRelevantForTradeOverview { get; set; }

        [JsonPropertyName("unitPriceRelevance")]
        public ValueRelevance UnitPriceRelevance { get; set; }

        [JsonPropertyName("totalCostRelevance")]
        public ValueRelevance TotalCostRelevance { get; set; }

        [JsonPropertyName("filterId")]
        public FilterId FilterId { get; set; } = FilterId.None;
    }
}
