using DayJT.Journal.Data;
using DayJTrading.Journal.Data.Factory;
using System.ComponentModel.DataAnnotations;

namespace DayJT.Journal.DataEntities.Entities
{
    public class TradeElement
    {
        [Key]
        public int Id { get; private set; }

        [Required]
        public TradeActionType TradeActionType { get; set; }

        public List<DataElement> Entries { get; set; } = new List<DataElement>();

        [Required]
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

        [Required]
        public int TradeCompositeFK { get; set; }

        [Required]
        public TradeComposite TradeCompositeRef { get; set; } = null!;
    }
}
