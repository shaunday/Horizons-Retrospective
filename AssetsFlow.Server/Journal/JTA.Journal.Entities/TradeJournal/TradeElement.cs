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

        [Required]
        public DateTime TimeStamp { get; set; }

        [Required]
        public bool IsActive { get; private set; } = false;

        #region Ctors
        public TradeElement() { }

        [SetsRequiredMembers]
        public TradeElement(TradeComposite trade, TradeActionType actionType)
        {
            CompositeRef = trade;
            TradeActionType = actionType;
        } 
        #endregion

        public void Activate()
        {
            IsActive = true;
            TimeStamp = DateTime.Now;
        }

        [Required]
        public int CompositeFK { get; set; }

        [Required]
        public required TradeComposite CompositeRef { get; set; } = null!;

        public override string ToString() =>  return $"Id={Id}, Typr={TradeActionType}, Entries Count={Entries.Count}";
    }
}
