using HsR.Journal.Entities;
using HsR.Journal.Entities.TradeJournal;
using HsR.Journal.Services;

namespace HsR.Journal.DataContext
{
    public interface ITradeElementRepository
    {
        Task<InterimTradeElement> AddInterimPositionAsync(int tradeId, bool isAdd);
        Task<InterimTradeElement> AddInterimEvalutationAsync(int tradeId);

        Task<TradeComposite> RemoveInterimPositionAsync(int tradeInputId);
        Task<UpdatedStatesCollation> UpdateActivationTimeAsync(int tradeInputId, string newTimestamp);
    }
}