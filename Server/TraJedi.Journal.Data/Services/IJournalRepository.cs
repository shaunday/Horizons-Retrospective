namespace TraJedi.Journal.Data.Services
{
    public interface IJournalRepository
    {
        //trades

        Task<IEnumerable<TradeComponent>> GetAllTradeComponentsAsync();

        Task<(IEnumerable<TradePositionComposite>, PaginationMetadata)> GetAllTradeCompositesAsync(int pageNumber = 1, int pageSize = 10);

        Task<TradePositionComposite> AddTradeCompositeAsync();

        //inputs

        Task<(TradeComponent newEntry, TradeComponent summary)> NewEntryAddPositionAsync(string tradeId);

        Task<(TradeComponent newEntry, TradeComponent summary)> NewEntryReducePositionAsync(string tradeId);

        Task<(bool result, TradeComponent? summary)> RemoveInterimEntry(string tradeInputId);

        //components

        Task<Cell?> UpdateCellContent(string componentId, string newContent, string changeNote);

        //closure

        Task Close(string tradeId, string closingPrice);

    }
}
