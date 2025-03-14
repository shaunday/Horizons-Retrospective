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

        public TradeElementModel? Summary { get; set; }

        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TradeStatus Status { get; set; }

        public bool IsPending { get; set; }

        public DateTime? OpenedAt { get; set; }
        public DateTime? ClosedAt { get; set; }
    }

    public class TradeCompositeModel : TradeCompositeInfo
    {
        public ICollection<TradeElementModel> TradeElements { get; set; } = null!;
    }
}
