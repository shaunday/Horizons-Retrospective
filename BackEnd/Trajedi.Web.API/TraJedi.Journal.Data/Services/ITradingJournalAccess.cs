namespace TraJedi.Journal.Data.Services
{
    public interface ITradingJournalAccess
    {
        ICollection<TradeWrapper> Trades { get; }

        TradeWrapper AddTrade();

        IEnumerable<string> GetTradeIds();

        TradeWrapper? GetTrade(string tradeId);
    }
}
