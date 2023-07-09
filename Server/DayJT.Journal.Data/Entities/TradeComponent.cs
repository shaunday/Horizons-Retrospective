using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DayJT.Journal.Data
{
    public class TradeComponent
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(50)]
        public TradeActionType TradeActionType { get; set; }

        public List<Cell> TradeActionInfoCells { get; set; } = new List<Cell>();


        //parent
        [ForeignKey(nameof(TradePositionCompositeRef))]
        public Guid TradePositionCompositeRefId { get; set; }

        public TradePositionComposite TradePositionCompositeRef { get; set; }

    }
}
