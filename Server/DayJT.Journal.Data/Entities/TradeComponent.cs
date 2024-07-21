using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DayJT.Journal.Data
{
    public class TradeComponent
    {
        [Key]
        public Guid Id { get; private set; } = Guid.NewGuid();

        [Required]
        [MaxLength(50)]
        public TradeActionType TradeActionType { get; set; }

        public List<Cell> TradeActionInfoCells { get; set; } = new List<Cell>();

        public DateTime CreatedAt { get; } = DateTime.Now;

        //parent
        public Guid TradePositionCompositeRefId { get; set; }

        public TradePositionComposite TradePositionCompositeRef { get; set; } = null!; // Required reference navigation to principal

    }
}
