using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace HsR.Journal.Entities
{
    public class TradeAction : TradeElement
    {
        [Required]
        public bool IsActive { get; private set; } = false;

        public TradeAction() : base() { }

        [SetsRequiredMembers]
        public TradeAction(TradeComposite trade, TradeActionType actionType) : base(trade, actionType) { }

        public void Activate()
        {
            IsActive = true;
            TimeStamp = DateTime.Now;
        }
    }
}
