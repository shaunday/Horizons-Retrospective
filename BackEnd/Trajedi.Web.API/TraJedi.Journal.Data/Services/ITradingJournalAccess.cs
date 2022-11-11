namespace TraJedi.Journal.Data.Services
{
    public interface ITradingJournalAccess
    {
        ICollection<TradeConstructAccess> Trades { get; }

        TradeConstructAccess AddTrade();

        IEnumerable<string> GetTradeIds();

        TradeConstructAccess? GetTrade(string tradeId);
    }
}
