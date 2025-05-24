using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace HsR.Journal.Entities
{
    public class InterimTradeElement : TradeElement
    {
        public bool IsActive => TimeStamp != null;

        public InterimTradeElement() : base() { }

        [SetsRequiredMembers]
        public InterimTradeElement(TradeComposite trade, TradeActionType actionType) : base(trade, actionType) { }

        public void Activate()
        {
            TimeStamp = DateTime.UtcNow;
        }
    }
}
