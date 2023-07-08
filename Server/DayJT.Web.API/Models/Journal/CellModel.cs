using System.ComponentModel.DataAnnotations;
using DayJT.Journal.Data;

namespace DayJT.Web.API.Models
{
    public class CellModel
    {
        public Guid Id { get; set; }

        public string Title { get; set; }
        public ComponentType ComponentType { get; set; }

        #region Content
        public CellContentModel ContentWrapper { get; set; }

        public string Content
        {
            get { return ContentWrapper.Content; }
            set { ContentWrapper.Content = value; }
        }
        #endregion

        #region flags

        public ValueRelevance CostRelevance { get; set; }

        public ValueRelevance PriceRelevance { get; set; }

        public bool IsRelevantForOverview { get; set; } = false;

        #endregion

    }
}
