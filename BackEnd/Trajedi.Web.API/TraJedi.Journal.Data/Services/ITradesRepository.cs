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

        Task<TradeInputModel?> GetTradeSummaryAsync(string tradeId);

        Task<bool> RemoveEntry(string tradeInputId);

        //components

        Task<InputComponentModel?> UpdateTradeInputComponent(string componentId, string newContent);

    }
}
