using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TraJedi.Journal.Data.Interfaces;

namespace TraJedi.Journal.Data
{
    public class TradingJournalAccess : ITradingJournalAccess
    {
        private readonly TradingJournalDataContext dataContext;

        public List<TradeWrapper> Trades { get; }

        #region Ctor

        public TradingJournalAccess(TradingJournalDataContext journalDbContext)
        {
            journalDbContext = journalDbContext ?? throw new ArgumentNullException(nameof(journalDbContext));
            Trades = new List<TradeWrapper>();

            //todo parse trades from the db
        }
        #endregion

        public TradeWrapper AddTrade()
        {
            TradeWrapper newTrade = new TradeWrapper(dataContext);
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
