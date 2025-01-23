using System.ComponentModel.DataAnnotations;

namespace HsR.Journal.Entities
{
    public class ContentRecord
    {
        // Private parameterless constructor for EF Core
        private ContentRecord() { }

        public ContentRecord(string content)
        {
            ContentValue = content;
        }

        [Required]
        public string ContentValue { get; set; } = string.Empty;

        public string? ChangeNote { get; set; }

        [Required]
        public DateTime CreatedAt { get; } = DateTime.Now;

        [Required]
        public int DataElementFK { get; set; }  // FK property to the owning DataCell
    }
}
