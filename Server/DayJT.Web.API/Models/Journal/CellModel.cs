using System.ComponentModel.DataAnnotations;
using DayJT.Journal.Data;

namespace DayJT.Web.API.Models
{
    public class CellModel
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = string.Empty;
        public ComponentType ComponentType { get; set; }

        public CellContentModel? ContentWrapper { get; set; }

        public ICollection<CellContent> History { get; set; } = new List<CellContent>();

        #region flags

        public ValueRelevance CostRelevance { get; set; }

        public ValueRelevance PriceRelevance { get; set; }

        public bool IsRelevantForOverview { get; set; } = false;

        #endregion

    }
}
