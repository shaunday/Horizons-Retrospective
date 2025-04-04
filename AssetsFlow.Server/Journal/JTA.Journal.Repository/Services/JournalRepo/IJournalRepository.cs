using HsR.Common;
using HsR.Journal.Entities;
using HsR.Journal.Services;

namespace HsR.Journal.DataContext
{
    public interface IJournalRepository
    {
        Task<(IEnumerable<TradeComposite>, PaginationMetadata)> GetAllTradeCompositesAsync(int pageNumber = 1, int pageSize = 10);
        Task<(IEnumerable<TradeComposite>, PaginationMetadata)> GetFilteredTradesAsync(TradesFilterModel filter, int pageNumber = 1, int pageSize = 10);

        Task<TradeComposite> AddTradeCompositeAsync();
    } 
}
