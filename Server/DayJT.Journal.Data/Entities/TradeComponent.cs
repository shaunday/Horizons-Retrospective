using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DayJT.Journal.Data
{
    public class TradeComponent
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; } = new Guid();

        [Required]
        [MaxLength(50)]
        public TradeActionType TradeActionType { get; set; }

        public List<Cell> TradeActionInfoCells { get; set; } = new List<Cell>();

        public DateTime CreatedAt { get; } = DateTime.Now;

        //parent
        public Guid TradePositionCompositeRefId { get; set; }

        public TradePositionComposite TradePositionCompositeRef { get; set; }

    }
}
