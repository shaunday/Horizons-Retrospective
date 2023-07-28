namespace DayJT.Journal.Data.Services
{
    public interface IJournalRepository
    {
        //trades

        Task<IEnumerable<TradeComponent>> GetAllTradeCompositesAs1LinerOverviewAsync();

        Task<(IEnumerable<TradePositionComposite>, PaginationMetadata)> GetAllTradeCompositesAsync(int pageNumber = 1, int pageSize = 10);

        Task<TradePositionComposite> AddTradeCompositeAsync();

        //inputs

        Task<(TradeComponent? newEntry, TradeComponent? summary)> NewEntryAddPositionAsync(string tradeId);

        Task<(TradeComponent? newEntry, TradeComponent? summary)> NewEntryReducePositionAsync(string tradeId);

        Task<(bool result, TradeComponent? summary)> RemoveInterimEntry(string tradeId, string tradeInputId);

        //components

        Task<(Cell? updatedCell, TradeComponent? summary)> UpdateCellContent(string componentId, string newContent, string changeNote);

        //closure

        Task CloseAsync(string tradeId, string closingPrice);

    }
}
