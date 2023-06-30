namespace TraJedi.Journal.Data.Services
{
    public interface IJournalRepository
    {
        //trades

        Task<IEnumerable<TradeInfoSingleLine>> GetAllTradeInfoLinesAsync();

        Task<(IEnumerable<TradePositionComposite>, PaginationMetadata)> GetAllTradeCompositesAsync(int pageNumber = 1, int pageSize = 10);

        Task<TradePositionComposite> AddTradeCompositeAsync();

        //inputs

        Task<(TradeInfoSingleLine newEntry, TradeInfoSingleLine summary)> NewEntryAddPositionAsync(string tradeId);

        Task<(TradeInfoSingleLine newEntry, TradeInfoSingleLine summary)> NewEntryReducePositionAsync(string tradeId);

        Task<(bool result, TradeInfoSingleLine? summary)> RemoveInterimEntry(string tradeInputId);

        //components

        Task<Cell?> UpdateCellContent(string componentId, string newContent, string changeNote);

        //closure

        Task Close(string tradeId, string closingPrice);

    }
}
