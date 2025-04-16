using HsR.Journal.Entities;
using HsR.Journal.Entities.TradeJournal;
using HsR.Journal.Services;

namespace HsR.Journal.DataContext
{
    public interface ITradeElementRepository
    {
        Task<(InterimTradeElement newEntry, UpdatedStatesCollation? updatedStates)> AddInterimPositionAsync(string tradeId, bool isAdd);
        Task<(InterimTradeElement newEntry, UpdatedStatesCollation? updatedStates)> AddInterimEvalutationAsync(string tradeId);

        Task<UpdatedStatesCollation> RemoveInterimPositionAsync(string tradeInputId);
        Task<UpdatedStatesCollation> UpdateActivationTimeAsync(string tradeInputId, string newTimestamp);
    }

}