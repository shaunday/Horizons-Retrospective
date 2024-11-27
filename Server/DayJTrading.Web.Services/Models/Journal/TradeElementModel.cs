using DayJT.Journal.Data;

namespace DayJT.Web.API.Models
{
    public class TradeElementModel
    {
        public int Id { get; set; }
        public TradeActionType TradeActionType { get; set; }
        public List<CellModel> Entries { get; set; } = null!;

        public int TradeCompositeFK { get; set; }
    }
}
