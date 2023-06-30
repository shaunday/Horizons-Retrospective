using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TraJedi.Journal.Data
{
    public class TradeInfoSingleLine
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(50)]
        public TradeActionType TradeActionType { get; set; }

        public List<Cell> TradeActionInfoCells { get; set; } = new List<Cell>();


        //parent
        [ForeignKey("TradePositionCompositeId")]
        public TradePositionComposite TradePositionComposite { get; set; } = null!;

        public Guid TradePositionCompositeId { get; set; }

    }
}
