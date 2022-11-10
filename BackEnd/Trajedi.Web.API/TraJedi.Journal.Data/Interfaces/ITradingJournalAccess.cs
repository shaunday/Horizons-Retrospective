using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraJedi.Journal.Data.Interfaces
{
    public interface ITradingJournalAccess
    {
        List<TradeWrapper> Trades { get; }

        TradeWrapper AddTrade();

        IEnumerable<string> GetTradeIds();

        TradeWrapper? GetTrade(string tradeId);
    }
}
