using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DayJT.Journal.Data;

namespace DayJT.Journal.Data
{
    public class ContentRecord
    {
        [Key]
        public int Id { get; private set; }

        [Required]
        public string Content { get; set; } = string.Empty;

        public string ChangeNote { get; set; } = string.Empty;

        public DateTime CreatedAt { get; } = DateTime.Now;

        public ContentRecord(Cell parentCell, string content)
        {
            Content = content;
            CellRef = parentCell;
        }

        //parent
        [Required]
        public int CellFK { get; set; }

        [Required]
        public Cell CellRef { get; set; } = null!;
    }
}
