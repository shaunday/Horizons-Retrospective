using HsR.Journal.Entities;
using HsR.Journal.Entities.TradeJournal;

namespace HsR.Journal.DataContext
{
    public interface ITradeElementRepository
    {
        Task<(TradeAction newEntry, TradeSummary summary)> AddInterimPositionAsync(string tradeId, bool isAdd);
        Task<TradeSummary> RemoveInterimPositionAsync(string tradeId, string tradeInputId);

        Task<TradeAction> ActivateTradeElement(string tradeEleId);
    }

}