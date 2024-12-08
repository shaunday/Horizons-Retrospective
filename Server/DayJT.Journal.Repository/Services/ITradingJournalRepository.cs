using DayJT.Journal.DataEntities.Entities;
using DayJTrading.Journal.Data;

namespace DayJT.Journal.DataContext.Services
{
    public interface ITradingJournalRepository
    {
        //composites
        Task<(IEnumerable<TradeComposite>, PaginationMetadata)> GetAllTradeCompositesAsync(int pageNumber = 1, int pageSize = 10);
        Task<TradeComposite> AddTradeCompositeAsync();
        Task<(IEnumerable<TradeComposite>, PaginationMetadata)> GetAllFilteredTradeCompositesAsync(DataFilteringInfo filter, int pageNumber = 1, int pageSize = 10);

        //interim elements
        Task<(TradeElement newEntry, TradeElement summary)> AddInterimPositionAsync(string tradeId, bool isAdd);
        Task<TradeElement> RemoveInterimPositionAsync(string tradeId, string tradeInputId);

        //components
        Task<(Cell updatedCell, TradeElement? summary)> UpdateCellContentAsync(string componentId, string newContent, string changeNote);

        //closure
        Task<TradeElement> CloseTradeAsync(string tradeId, string closingPrice);

        //data
        Task<List<string>?> GetAllSavedSectors();
    }
}
