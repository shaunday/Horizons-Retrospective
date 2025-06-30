using HsR.Journal.Entities;
using HsR.Journal.Entities.TradeJournal;
using HsR.Journal.Services;

namespace HsR.Journal.DataContext
{
    public interface ITradeElementRepository
    {
        Task<(InterimTradeElement newEntry, UpdatedStatesCollation? updatedStates)> AddInterimPositionAsync(Guid userId, string tradeId, bool isAdd);
        Task<(InterimTradeElement newEntry, UpdatedStatesCollation? updatedStates)> AddInterimEvalutationAsync(Guid userId, string tradeId);

        Task<UpdatedStatesCollation> RemoveInterimPositionAsync(Guid userId, string tradeInputId);
        Task<UpdatedStatesCollation> UpdateActivationTimeAsync(Guid userId, string tradeInputId, string newTimestamp);
    }
}