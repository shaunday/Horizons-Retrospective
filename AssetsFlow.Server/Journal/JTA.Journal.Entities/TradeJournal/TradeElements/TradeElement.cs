using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace HsR.Journal.Entities
{
    public class TradeElement
    {
        [Key]
        public int Id { get; private set; }

        [Required]
        public required TradeActionType TradeActionType { get; set; }

        public List<DataElement> Entries { get; set; } = new List<DataElement>();

        public DateTime? TimeStamp { get; set; }

        #region Ctors
        public TradeElement() { }

        [SetsRequiredMembers]
        public TradeElement(TradeComposite trade, TradeActionType actionType)
        {
            CompositeRef = trade;
            CompositeFK = trade.Id;
            TradeActionType = actionType;
        } 
        #endregion

        [Required]
        public int CompositeFK { get; set; }

        [Required]
        public required TradeComposite CompositeRef { get; set; } = null!;

        public override string ToString() => $"Id={Id}, Typr={TradeActionType}, Entries Count={Entries.Count}";
    }
}
