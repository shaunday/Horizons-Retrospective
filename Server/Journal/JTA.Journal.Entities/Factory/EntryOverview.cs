namespace JTA.Journal.Entities.Factory
{
    internal class EntryOverview(string title, ComponentType componentType, string content = "")
    {
        public string Title { get; set; } = title;
        public string Content { get; set; } = content;
        public ComponentType Type { get; set; } = componentType;

        public ValueRelevance CostRelevance { get; set; }
        public ValueRelevance ValueRelevance { get; set; }
    }
}
