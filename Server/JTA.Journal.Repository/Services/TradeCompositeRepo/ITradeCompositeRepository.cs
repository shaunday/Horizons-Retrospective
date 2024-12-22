using JTA.Common;
using JTA.Journal.Entities;

namespace JTA.Journal.DataContext
{
    public interface ITradeCompositeRepository
    {
        Task<(IAsyncEnumerable<TradeComposite>, PaginationMetadata)> GetAllTradeCompositesAsync(int pageNumber = 1, int pageSize = 10);
        Task<TradeComposite> AddTradeCompositeAsync();
        Task<(IAsyncEnumerable<TradeComposite>, PaginationMetadata)> GetFilteredTradesAsync(TradesFilterModel filter, int pageNumber = 1, int pageSize = 10);

    } 
}
