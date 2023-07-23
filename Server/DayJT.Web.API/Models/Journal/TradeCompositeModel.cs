using DayJT.Journal.Data;

namespace DayJT.Web.API.Models
{
    public class TradePositionCompositeModel
    {
        public Guid Id { get; set; }
        public ICollection<TradeComponentModel>? TradeComponents { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
