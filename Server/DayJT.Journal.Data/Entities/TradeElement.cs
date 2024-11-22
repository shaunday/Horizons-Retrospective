using DayJTrading.Journal.Data.Factory;
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

        public TradeElement(TradeComposite trade, TradeActionType actionType)
        {
            TradeCompositeRef = trade;

            if (actionType == TradeActionType.Origin || actionType == TradeActionType.AddPosition || actionType == TradeActionType.ReducePosition)
            {
                TradeActionType = actionType;
                Entries = InterimPositionFactory.GetPositionEntries(actionType, this);
            }
        }

        //parent
        [Required]
        public int TradeCompositeFK { get; set; }

        [Required]
        public TradeComposite TradeCompositeRef { get; set; } = null!; 

    }
}
