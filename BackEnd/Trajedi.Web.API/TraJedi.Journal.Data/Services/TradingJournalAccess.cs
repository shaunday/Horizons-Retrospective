namespace TraJedi.Journal.Data.Services
{
    public class TradingJournalAccess : ITradingJournalAccess
    {
        private readonly TradingJournalDataContext dataContext;

        public ICollection<TradeConstructAccess> Trades { get; }

        #region Ctor

        public TradingJournalAccess(TradingJournalDataContext journalDbContext)
        {
            dataContext = journalDbContext ?? throw new ArgumentNullException(nameof(journalDbContext));
            Trades = new List<TradeConstructAccess>();

            //todo parse trades from the db
        }
        #endregion

        public TradeConstructAccess AddTrade()
        {
            TradeConstructAccess newTrade = new TradeConstructAccess(dataContext);
            return newTrade;
        }

        public IEnumerable<string> GetTradeIds()
        {
            return Trades.Select(t => t.GetIdString());
        }

        public TradeConstructAccess? GetTrade(string tradeId)
        {
            return Trades.FirstOrDefault(t => t.GetIdString() == tradeId);
        }
    }
}
