using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace DayJT.Journal.Data
{
    public class TradeElement
    {
        [Key]
        public int Id { get; private set; } 

        [Required]
        public TradeActionType TradeActionType { get; set; }

        public List<Cell> Entries { get; set; } = new List<Cell>();

        public DateTime CreatedAt { get; } = DateTime.Now;

        public TradeElement() { }

        public TradeElement(TradeComposite trade)
        {
            TradeCompositeRef = trade;
            TradeCompositeRefId = trade.Id;
        }

        //parent
        [Required]
        public int TradeCompositeRefId { get; set; }

        [Required]
        public TradeComposite TradeCompositeRef { get; set; } = null!; // Required reference navigation to principal

    }
}
