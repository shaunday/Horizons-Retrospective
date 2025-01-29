using HsR.Common;
using HsR.Journal.Entities;

namespace HsR.Journal.DataContext
{
    public interface ITradeCompositesRepository
    {
        Task<(IEnumerable<TradeComposite>, PaginationMetadata)> GetAllTradeCompositesAsync(int pageNumber = 1, int pageSize = 10);
        Task<(IEnumerable<TradeComposite>, PaginationMetadata)> GetFilteredTradesAsync(TradesFilterModel filter, int pageNumber = 1, int pageSize = 10);

        Task<TradeComposite> AddTradeCompositeAsync();
        Task<TradeElement> CloseTradeAsync(string tradeId, string closingPrice);
    } 
}
