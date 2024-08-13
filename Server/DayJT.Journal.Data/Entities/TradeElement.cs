using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DayJT.Journal.Data
{
    public class TradeElement
    {
        [Key]
        public Guid Id { get; private set; } = Guid.NewGuid();

        [Required]
        [MaxLength(50)]
        public TradeActionType TradeActionType { get; set; }

        public List<Cell> Entries { get; set; } = new List<Cell>();

        public DateTime CreatedAt { get; } = DateTime.Now;

        //parent
        public Guid TradeCompositeRefId { get; set; }

        public TradeComposite TradeCompositeRef { get; set; } = null!; // Required reference navigation to principal

    }
}
