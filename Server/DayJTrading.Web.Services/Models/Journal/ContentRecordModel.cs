namespace DayJT.Web.API.Models
{
    public class ContentRecordModel
    {
        public string Content { get; set; } = string.Empty;

        public string ChangeNote { get; set; } = string.Empty;

        public DateTime CreatedAt { get; } = DateTime.Now;

    }
}
