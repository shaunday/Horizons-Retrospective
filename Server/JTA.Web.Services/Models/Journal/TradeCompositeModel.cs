using DayJT.Journal.DataEntities.Entities;
using JTA.Journal.Entities;
using JTA.Web.Services.Models.Journal;
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

        public TradeStatusModel Status { get; set; } = null!;
    }
}
