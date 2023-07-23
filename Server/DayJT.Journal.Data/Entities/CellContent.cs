using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DayJT.Journal.Data;

namespace DayJT.Journal.Data
{
    public class CellContent
    {
        [Key]
        public Guid Id { get; private set; } = Guid.NewGuid();

        public string Content { get; set; } = string.Empty;

        public string ChangeNote { get; set; } = string.Empty;

        public DateTime CreatedAt { get; } = DateTime.Now;

        //parent
        public Guid? CellRefId { get; set; } //reference is optional (for main content)

        public Cell? CellRef { get; set; }
    }
}
