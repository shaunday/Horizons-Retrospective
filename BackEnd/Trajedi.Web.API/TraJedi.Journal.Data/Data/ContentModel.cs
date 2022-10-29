namespace TraJedi.Journal.Data
{
    public class ContentModel
    {
        public Guid Id { get; set; }

        public string Content { get; set; } = string.Empty;

        public DateTime AddedAt { get; } = DateTime.Now;
    }
}
