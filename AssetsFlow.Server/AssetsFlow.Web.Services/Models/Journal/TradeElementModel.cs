using HsR.Common.Extenders;
using HsR.Journal.Entities;
using HsR.Web.Services.Models.Journal;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AssetsFlowWeb.Services.Models.Journal
{
    public class TradeElementModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TradeActionType TradeActionType { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public bool IsAnyContentMissing { get; set; }

        [Required]
        public List<DataElementModel> Entries { get; set; } = null!;

        public DateTime? TimeStamp { get; set; }

        [Required]
        public int CompositeFK { get; set; }

        public override string ToString() => $"Id={Id}, Type={TradeActionType}, Entries Count={Entries.Count}";
    }
}
