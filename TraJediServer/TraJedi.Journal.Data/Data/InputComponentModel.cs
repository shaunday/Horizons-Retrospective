using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace TraJedi.Journal.Data
{
    public class InputComponentModel
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public ComponentType ComponentType { get; set; }

        #region Content
        public ContentModel ContentWrapper { get; set; } = new ContentModel();

        public string Content
        {
            get { return ContentWrapper.Content; }
            set { ContentWrapper.Content = value; }
        } 
        #endregion

        #region flags

        public ValueRelevance CostRelevant { get; set; } = ValueRelevance.None;

        public ValueRelevance PriceValueRelevant { get; set; } = ValueRelevance.None;

        public bool AttachedToggle { get; set; } = false;

        public bool RelevantForTradeSummary { get; set; } = false;

        #endregion

        public List<ContentModel> History { get; set; } = new List<ContentModel>();

    }

}
