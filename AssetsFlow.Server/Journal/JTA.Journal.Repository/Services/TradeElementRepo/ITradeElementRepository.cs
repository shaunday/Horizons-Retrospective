using HsR.Journal.Entities;
using HsR.Journal.Entities.TradeJournal;
using HsR.Journal.Services;

namespace HsR.Journal.DataContext
{
    public interface ITradeElementRepository
    {
        Task<(InterimTradeElement newEntry, UpdatedStatesCollation? updatedStates)> AddInterimPositionAsync(string tradeId, bool isAdd);
        Task<InterimTradeElement> AddInterimEvalutationAsync(string tradeId);

        Task<UpdatedStatesCollation> RemoveInterimPositionAsync(string tradeInputId);
    }

}