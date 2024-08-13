using DayJT.Journal.Data;

namespace DayJT.Web.API.Models
{
    public class TradeCompositeModel
    {
        public Guid Id { get; set; }
        public ICollection<TradeElementModel>? TradeElements { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
