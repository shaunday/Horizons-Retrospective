using System.ComponentModel.DataAnnotations;

namespace DayJT.Journal.DataEntities.Entities
{
    public class ContentRecord(string content)
    {
        [Required]
        public string ContentValue { get; set; } = content;

        public string? ChangeNote { get; set; }

        [Required]
        public DateTime CreatedAt { get; } = DateTime.Now;

        [Required]
        public int DataElementFK { get; set; }  // FK property to the owning DataCell
    }
}
