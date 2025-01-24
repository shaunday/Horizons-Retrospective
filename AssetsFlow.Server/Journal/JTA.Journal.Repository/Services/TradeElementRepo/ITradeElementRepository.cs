using HsR.Journal.Entities;

namespace HsR.Journal.DataContext
{
    public interface ITradeElementRepository
    {
        Task<TradeElement> CloseTradeAsync(string tradeId, string closingPrice);
        Task<(TradeElement newEntry, TradeElement summary)> AddInterimPositionAsync(string tradeId, bool isAdd);
        Task<TradeElement> RemoveInterimPositionAsync(string tradeId, string tradeInputId);
    }

}