using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HsR.Journal.Entities.TradeJournal
{
    public class TradeSummary : TradeElement
    {
        public TradeSummary() : base() { }

        [SetsRequiredMembers]
        public TradeSummary(TradeComposite trade, TradeActionType actionType) : base(trade, actionType) { }
    }
}
