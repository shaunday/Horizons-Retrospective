using DayJT.Journal.Data;

namespace DayJT.Journal.DataContext.Services
{
    public interface IJournalRepository
    {
        //trades

        //Task<IEnumerable<TradeElement>> GetAllTradeCompositesAs1LinerOverviewAsync();

        Task<(IEnumerable<TradeComposite>, PaginationMetadata)> GetAllTradeCompositesAsync(int pageNumber = 1, int pageSize = 10);

        Task<TradeComposite> AddTradeCompositeAsync();

        //inputs

        Task<(TradeElement? element, TradeElement? summary)> GetTradeElement(string tradeId, string tradeElementId);

        Task<(TradeElement? newEntry, TradeElement? summary)> AddPositionAsync(string tradeId);

        Task<(TradeElement? newEntry, TradeElement? summary)> ReducePositionAsync(string tradeId);

        Task<(bool result, TradeElement? summary)> RemoveInterimEntry(string tradeId, string tradeInputId);

        //components

        Task<(Cell? updatedCell, TradeElement? summary)> UpdateCellContent(string componentId, string newContent, string changeNote);

        //closure

        Task<TradeElement> CloseAsync(string tradeId, string closingPrice);

    }
}
