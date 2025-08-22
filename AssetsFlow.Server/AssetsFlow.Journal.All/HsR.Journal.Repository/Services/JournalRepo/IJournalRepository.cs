using HsR.Common;
using HsR.Journal.Entities;
using HsR.Journal.Services;

namespace HsR.Journal.DataContext
{
    public interface IJournalRepository
    {
        Task<(IEnumerable<TradeComposite>?, int totalTradesCount)> GetAllTradeCompositesAsync(Guid userId, int pageNumber = 1, int pageSize = 10);
        Task<(IEnumerable<TradeComposite>?, int totalTradesCount)> GetFilteredTradeCompositesAsync(Guid userId, IEnumerable<FilterDefinition>? filters,
                                                                                            int pageNumber = 1, int pageSize = 10);

        Task<TradeComposite> AddTradeCompositeAsync(Guid userId);

        Task<TradeComposite?> GetTradeCompositeByIdAsync(int tradeId);
    } 
}
