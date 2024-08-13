using DayJT.Journal.Data;

namespace DayJT.Web.API.Models
{
    public class TradeElementModel
    {
        public Guid Id { get; set; }
        public TradeActionType TradeActionType { get; set; }
        public List<CellModel>? Entries { get; set; }
    }
}
