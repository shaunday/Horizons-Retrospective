using JTA.Common;
using JTA.Journal.Entities;

namespace JTA.Journal.DataContext
{
    public interface ITradeCompositeRepository
    {
        Task<(IEnumerable<TradeComposite>, Pagination)> GetAllTradeCompositesAsync(int pageNumber = 1, int pageSize = 10);
        Task<TradeComposite> AddTradeCompositeAsync();
        Task<(IEnumerable<TradeComposite>, Pagination)> GetFilteredTradesAsync(TradesFilterModel filter, int pageNumber = 1, int pageSize = 10);

    } 
}
