using DayJT.Journal.Data;
using DayJT.Journal.DataEntities.Entities;
using JTA.Journal.Entities;
using System.ComponentModel.DataAnnotations;

namespace DayJTrading.Web.Services.Models.Journal
{
    public class TradeCompositeModel
    {
        [Required]
        public int Id { get; set; }
        public ICollection<TradeElementModel> TradeElements { get; set; } = null!;
        public TradeElementModel Summary { get; set; } = null!;

        public List<string> Sectors { get; set; } = null!;

        [Required]
        public TradeStatus Status { get; set; } 

        public DateTime? OpenedAt { get; set; }

        public DateTime? ClosedAt { get; set; }
    }
}
