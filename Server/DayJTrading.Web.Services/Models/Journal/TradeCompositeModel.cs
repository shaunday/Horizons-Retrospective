using DayJT.Journal.DataEntities.Entities;

namespace DayJTrading.Web.Services.Models.Journal
{
    public class TradeCompositeModel
    {
        public int Id { get; set; }
        public ICollection<TradeElementModel> TradeElements { get; set; } = null!;
        public TradeElement Summary { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public string Sector { get; set; } = "";
    }
}
