using DayJT.Journal.DataEntities.Entities;
using System.ComponentModel.DataAnnotations;

namespace DayJTrading.Web.Services.Models.Journal
{
    public class TradeCompositeModel
    {
        [Required]
        public int Id { get; set; }
        public ICollection<TradeElementModel> TradeElements { get; set; } = null!;
        public TradeElement Summary { get; set; } = null!;

        public List<string> Sectors { get; set; } = null!;
    }
}
