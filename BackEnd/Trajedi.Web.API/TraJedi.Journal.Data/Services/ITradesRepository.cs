namespace TraJedi.Journal.Data.Services
{
    public interface ITradesRepository
    {
        Task<IEnumerable<TradeInputModel>> GetAllTradeOverviewsAsync();

        Task<(IEnumerable<TradeConstruct>, PaginationMetadata)> GetAllTradesAsync(int pageNumber = 1, int pageSize = 10);

        Task<TradeConstruct> AddTradeAsync();

        Task<TradeInputModel> NewEntryAddPositionAsync(string tradeId);

        Task<TradeInputModel> NewEntryReducePositionAsync(string tradeId);

        Task<InputComponentModel?> UpdateTradeInputComponent(string componentId, string newContent);

        Task<bool> RemoveEntry(string tradeInputId);

    }
}
