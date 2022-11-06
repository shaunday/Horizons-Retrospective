using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraJedi.Journal.Data.Wrappers
{
    public class JournalWrapper
    {
        public List<TradeWrapper> Trades { get; }

        public static JournalWrapper Current { get; } = new JournalWrapper();

        public JournalWrapper()
        {
            Trades = new List<TradeWrapper>();
        }

        public TradeWrapper AddTrade()
        {
            TradeWrapper newTrade = new TradeWrapper();
            //trade.Init();
            return newTrade;
        }

        public IEnumerable<string> GetTradeIds()
        {
            return Trades.Select(t => t.GetIdString());
        }

        public TradeWrapper? GetTrade(string tradeId)
        {
            return Trades.FirstOrDefault(t => t.GetIdString() == tradeId);
        }
    }
}
