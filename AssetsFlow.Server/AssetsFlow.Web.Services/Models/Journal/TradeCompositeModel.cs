using HsR.Common.Extenders;
using HsR.Journal.Entities;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HsR.Web.Services.Models.Journal
{
    public class TradeCompositeModel
    {
        [Required]
        public int Id { get; set; }
        public ICollection<TradeElementModel> TradeElements { get; set; } = null!;

        public TradeElementModel? Summary { get; set; }

        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TradeStatus Status { get; set; }

        public bool IsPending { get; set; }

        [JsonIgnore]
        public DateTime? OpenedAt { get; set; }
        [JsonIgnore]
        public DateTime? ClosedAt { get; set; }

        public string? FormattedOpenedAt => OpenedAt?.ToTimeFormattedString();
        public string? FormattedClosedAt => ClosedAt?.ToTimeFormattedString();
    }
}
