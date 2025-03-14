using HsR.Common.Extenders;
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

        public DateTime? TimeStamp { get; set; }

        [Required]
        public int CompositeFK { get; set; }
    }
}
