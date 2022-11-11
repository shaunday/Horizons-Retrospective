namespace TraJedi.Journal.Data.Services
{
    public interface ITradingJournalAccess
    {
        List<TradeWrapper> Trades { get; }

        TradeWrapper AddTrade();

        IEnumerable<string> GetTradeIds();

        TradeWrapper? GetTrade(string tradeId);
    }
}
