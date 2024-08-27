using DayJT.Journal.Data;

namespace DayJT.Journal.DataContext.Services
{
    public interface IJournalRepository
    {
        //composites

        Task<TradeComposite> GetTradeCompositeByCounterAsync(string counter);

        Task<TradeComposite> AddTradeCompositeAsync();

        //interim elements

        Task<(TradeElement newEntry, TradeElement summary)> AddInterimPositionAsync(string tradeId, bool isAdd);

        Task<TradeElement> RemoveInterimPositionAsync(string tradeId, string tradeInputId);

        //components

        Task<(Cell updatedCell, TradeElement? summary)> UpdateCellContent(string componentId, string newContent, string changeNote);

        //closure

        Task<TradeElement> CloseAsync(string tradeId, string closingPrice);

    }
}
