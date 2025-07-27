using HsR.Common.Extenders;
using HsR.Journal.Entities;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HsR.Web.Services.Models.Journal
{
    public class TradeCompositeInfo
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public Guid UserId { get; set; }

        public TradeElementModel? Summary { get; set; }

        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TradeStatus Status { get; set; }

        [Required]
        public bool IsAnyContentMissing { get; set; }

        public DateTime? OpenedAt { get; set; }
        public DateTime? ClosedAt { get; set; }
        public DateTime? IdeaDate { get; set; }

        public override string ToString() => $"Id={Id}, IsAnyContentMissing={IsAnyContentMissing}, Status={Status}";
    }

    public class TradeCompositeModel : TradeCompositeInfo
    {
        public ICollection<TradeElementModel> TradeElements { get; set; } = null!;

        public override string ToString() => $"Id={Id}, Status={Status}, Trade Eles Count={TradeElements.Count}";
    }
}
