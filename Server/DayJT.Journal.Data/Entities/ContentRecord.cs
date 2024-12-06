using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DayJT.Journal.Data;

namespace DayJT.Journal.Data
{
    public class ContentRecord
    {
        [Required]
        public string Content { get; set; } = string.Empty;

        public string ChangeNote { get; set; } = string.Empty;

        public DateTime CreatedAt { get; } = DateTime.Now;

        public ContentRecord(string content)
        {
            Content = content;
        }

        [Required]
        public Cell CellRef { get; set; } = null!; // Navigation property to the owning Cell
    }
}
