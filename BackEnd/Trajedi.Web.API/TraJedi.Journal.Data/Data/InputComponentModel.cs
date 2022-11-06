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

        public ValueRelevance CostRelevance { get; set; } = ValueRelevance.None;

        public ValueRelevance PriceValueRelevance { get; set; } = ValueRelevance.None;

        public bool AttachedToggle { get; set; } = false;

        public bool IsRelevantForOneLineSummation { get; set; } = false;

        #endregion

        public List<ContentModel> History { get; set; } = new List<ContentModel>();
    }

}
