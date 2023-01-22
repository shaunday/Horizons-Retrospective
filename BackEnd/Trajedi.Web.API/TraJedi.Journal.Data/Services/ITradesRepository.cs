namespace TraJedi.Journal.Data.Services
{
    public interface ITradesRepository
    {
        //trades

        Task<IEnumerable<TradeInputModel>> GetAllTradeOverviewsAsync();

        Task<(IEnumerable<TradeConstruct>, PaginationMetadata)> GetAllTradesAsync(int pageNumber = 1, int pageSize = 10);

        Task<TradeConstruct> AddTradeAsync();

        //inputs

        Task<(TradeInputModel newEntry, TradeInputModel summary)> NewEntryAddPositionAsync(string tradeId);

        Task<(TradeInputModel newEntry, TradeInputModel summary)> NewEntryReducePositionAsync(string tradeId);

        Task<(bool result, TradeInputModel? summary)> RemoveInterimEntry(string tradeInputId);

        //components

        Task<InputComponentModel?> UpdateTradeInputComponent(string componentId, string newContent, string changeNote);

        //closure

        Task Close(string tradeId, string closingPrice);

    }
}
