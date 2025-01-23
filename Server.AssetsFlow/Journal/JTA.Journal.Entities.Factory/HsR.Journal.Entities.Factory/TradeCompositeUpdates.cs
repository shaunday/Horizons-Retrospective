using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HsR.Journal.Entities.Factory
{
    public class TradeCompositeUpdates
    {
        public static TradeComposite CloseTrade(TradeComposite trade, string closingPrice)
        {
            var closureElement = TradeElementCRUDs.CreateTradeElementForClosure(trade, closingPrice);
            trade.Summary = closureElement;

            return trade;
        }

        public static TradeElement RecreateSummary(TradeComposite trade)
        {
            TradeElement summary = TradeElementCRUDs.GetInterimSummary(trade);
            trade.Summary = summary;

            return summary;
        }
    }
}
