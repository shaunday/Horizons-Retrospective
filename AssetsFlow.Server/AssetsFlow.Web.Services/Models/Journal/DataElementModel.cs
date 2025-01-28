using System.ComponentModel.DataAnnotations;
using HsR.Journal.Entities;

namespace HsR.Web.Services.Models.Journal
{
    public class DataElementModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public ComponentType ComponentType { get; set; }

        [Required]
        public ContentRecord? ContentWrapper { get; set; }
        public ICollection<ContentRecord>? History { get; set; }
        public ICollection<string>? Restrictions { get; set; }

        public ValueRelevance? TotalCostRelevance { get; set; }
        public ValueRelevance? UnitPriceRelevance { get; set; }
        public bool IsRelevantForOverview { get; set; }

        [Required]
        public int TradeElementFK { get; set; }
        [Required]
        public int TradeCompositeFK { get; set; }
    }
}
