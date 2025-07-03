using HsR.Journal.Entities;
using HsR.Journal.Entities.TradeJournal;
using HsR.Journal.Services;

namespace HsR.Journal.DataContext
{
    public interface ITradeElementRepository
    {
        Task<(InterimTradeElement newEntry, UpdatedStatesCollation? updatedStates)> AddInterimPositionAsync(int tradeId, bool isAdd);
        Task<(InterimTradeElement newEntry, UpdatedStatesCollation? updatedStates)> AddInterimEvalutationAsync(int tradeId);

        Task<UpdatedStatesCollation> RemoveInterimPositionAsync(int tradeInputId);
        Task<UpdatedStatesCollation> UpdateActivationTimeAsync(int tradeInputId, string newTimestamp);
    }
}