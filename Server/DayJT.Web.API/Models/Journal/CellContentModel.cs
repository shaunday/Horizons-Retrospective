namespace DayJT.Web.API.Models
{
    public class CellContentModel
    {
        public Guid Id { get; set; }

        public string Content { get; set; } = string.Empty;

        public string ChangeNote { get; set; } = string.Empty;

        public DateTime CreatedAt { get; } = DateTime.Now;
    }
}
