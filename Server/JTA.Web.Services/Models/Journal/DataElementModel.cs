using System.ComponentModel.DataAnnotations;
using DayJT.Journal.Data;
using DayJT.Journal.DataEntities.Entities;

namespace DayJTrading.Web.Services.Models.Journal
{
    public class DataElementModel
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public ComponentType ComponentType { get; set; }

        public ContentRecordModel ContentWrapper { get; set; } = null!;
        public ICollection<ContentRecord>? History { get; set; }

        public ValueRelevance CostRelevance { get; set; }
        public ValueRelevance PriceRelevance { get; set; }
        public bool IsRelevantForOverview { get; set; } = false;

        public int TradeElementFK { get; set; }
        public int TradeCompositeFK { get; set; }
    }
}
