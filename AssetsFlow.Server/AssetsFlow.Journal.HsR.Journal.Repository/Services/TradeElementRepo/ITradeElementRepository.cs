using System;
using System.Threading.Tasks;

namespace AssetsFlow.Server.AssetsFlow.Journal.HsR.Journal.Repository.Services.TradeElementRepo
{
    public interface ITradeElementRepository
    {
        Task<(InterimTradeElement newEntry, UpdatedStatesCollation? updatedStates)> AddInterimPositionAsync(Guid userId, int tradeId, bool isAdd);
        Task<(InterimTradeElement newEntry, UpdatedStatesCollation? updatedStates)> AddInterimEvalutationAsync(Guid userId, int tradeId);
        Task<UpdatedStatesCollation> RemoveInterimPositionAsync(int tradeInputId);
        Task<UpdatedStatesCollation> UpdateActivationTimeAsync(int tradeInputId, string newTimestamp);
    }
} 