using System.ComponentModel.DataAnnotations;

namespace DayJT.Journal.DataEntities.Entities
{
    public class ContentRecord
    {
        // Private parameterless constructor for EF Core
        private ContentRecord()
        {
            ContentValue = string.Empty; // Initialize to avoid CS8618
        }

        public ContentRecord(string content)
        {
            ContentValue = content;
        }

        [Required]
        public string ContentValue { get; set; }

        public string? ChangeNote { get; set; }

        [Required]
        public DateTime CreatedAt { get; } = DateTime.Now;

        [Required]
        public int DataElementFK { get; set; }  // FK property to the owning DataCell
    }
}
