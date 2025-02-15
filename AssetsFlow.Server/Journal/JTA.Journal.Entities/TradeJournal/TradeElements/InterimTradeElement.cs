using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace HsR.Journal.Entities
{
    public class InterimTradeElement : TradeElement
    {
        [Required]
        public bool IsActive { get; private set; } = false;

        public InterimTradeElement() : base() { }

        [SetsRequiredMembers]
        public InterimTradeElement(TradeComposite trade, TradeActionType actionType) : base(trade, actionType) { }

        public void Activate()
        {
            IsActive = true;
            TimeStamp = DateTime.Now;
        }
    }
}
