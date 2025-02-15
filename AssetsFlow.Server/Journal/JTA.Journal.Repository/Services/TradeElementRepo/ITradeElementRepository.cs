using HsR.Journal.Entities;
using HsR.Journal.Entities.TradeJournal;

namespace HsR.Journal.DataContext
{
    public interface ITradeElementRepository
    {
        Task<(InterimTradeElement newEntry, TradeSummary? summary)> AddInterimPositionAsync(string tradeId, bool isAdd);
        Task<InterimTradeElement> AddInterimEvalutationAsync(string tradeId);

        Task<TradeSummary?> RemoveInterimPositionAsync(string tradeId, string tradeInputId);

        Task<InterimTradeElement> ActivateTradeElement(string tradeEleId);
    }

}