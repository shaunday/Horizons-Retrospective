using HsR.Common;
using HsR.Journal.Entities;

namespace HsR.Journal.DataContext
{
    public interface ITradeCompositeRepository
    {
        Task<(IEnumerable<TradeComposite>, PaginationMetadata)> GetAllTradeCompositesAsync(int pageNumber = 1, int pageSize = 10);
        Task<TradeComposite> AddTradeCompositeAsync();
        Task<(IEnumerable<TradeComposite>, PaginationMetadata)> GetFilteredTradesAsync(TradesFilterModel filter, int pageNumber = 1, int pageSize = 10);

    } 
}
