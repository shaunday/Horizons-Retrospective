using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TraJedi.Journal.Data
{
    public class CellContent
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string Content { get; set; } = string.Empty;

        public string ChangeNote { get; set; } = string.Empty;

        public DateTime CreatedAt { get; } = DateTime.Now;

        //parent
        [ForeignKey("CellId")]
        public Cell? Cell { get; set; }

        public Guid CellId { get; set; }
    }
}
