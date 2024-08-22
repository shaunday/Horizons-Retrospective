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

        //parent
        [Required]
        public int CellRefId { get; set; } //reference is optional (for main content)

        [Required]
        public Cell CellRef { get; set; } = null!;
    }
}
