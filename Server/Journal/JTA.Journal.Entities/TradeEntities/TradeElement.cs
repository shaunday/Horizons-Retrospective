using JTA.Journal.Entities.Factory;
using System.ComponentModel.DataAnnotations;

namespace JTA.Journal.Entities
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

        #region Ctors
        public TradeElement() { }

        public TradeElement(TradeComposite trade, TradeActionType actionType)
        {
            CompositeRef = trade;

            if (actionType == TradeActionType.Origin || actionType == TradeActionType.AddPosition || actionType == TradeActionType.ReducePosition)
            {
                TradeActionType = actionType;
                Entries = InterimPositionFactory.GetPositionEntries(actionType, this);
            }
        } 
        #endregion

        [Required]
        public int CompositeFK { get; set; }

        [Required]
        public TradeComposite CompositeRef { get; set; } = null!;
    }
}
