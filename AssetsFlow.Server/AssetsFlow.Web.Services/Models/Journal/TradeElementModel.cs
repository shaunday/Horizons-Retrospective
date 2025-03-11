using HsR.Journal.Entities;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HsR.Web.Services.Models.Journal
{
    public class TradeElementModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TradeActionType TradeActionType { get; set; }

        [Required]
        public List<DataElementModel> Entries { get; set; } = null!;

        [JsonIgnore]
        public DateTime? TimeStamp { get; set; }

        public string? FormattedTimeStamp => TimeStamp?.ToString("yyyy-MM-dd HH:mm");

        [Required]
        public int CompositeFK { get; set; }
    }
}
