using System.ComponentModel.DataAnnotations;

namespace HsR.Journal.Entities
{
    public class TradeElement
    {
        [Key]
        public int Id { get; private set; }

        [Required]
        public TradeActionType TradeActionType { get; set; }

        public List<DataElement> Entries { get; set; } = [];

        [Required]
        public DateTime CreatedAt { get; } = DateTime.Now;

        #region Ctors
        public TradeElement() { }

        public TradeElement(TradeComposite trade, TradeActionType actionType)
        {
            CompositeRef = trade;
            TradeActionType = actionType;
        } 
        #endregion

        [Required]
        public int CompositeFK { get; set; }

        [Required]
        public TradeComposite CompositeRef { get; set; } = null!;
    }
}
