using DayJT.Journal.Data;

namespace DayJTrading.Web.Services.Models.Journal
{
    public class TradeElementModel
    {
        public int Id { get; set; }
        public TradeActionType TradeActionType { get; set; }
        public List<DataElementModel> Entries { get; set; } = null!;

        public int TradeCompositeFK { get; set; }
    }
}
