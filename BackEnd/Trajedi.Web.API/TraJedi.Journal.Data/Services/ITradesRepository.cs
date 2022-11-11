namespace TraJedi.Journal.Data.Services
{
    public interface ITradesRepository
    {
        Task<IEnumerable<TradeInputModel>> GetAllTradesOneLinerSummariesAsync();

        Task<IEnumerable<TradeConstruct>> GetAllTradesAsync();

        Task<TradeConstruct> AddTradeAsync();

        Task<TradeInputModel> AddTradeEntryAsync(string tradeId);

        Task<TradeInputModel> AddTradeExitAsync(string tradeId);

        Task<InputComponentModel?> UpdateTradeInputComponent(string componentId, string newContent);
    }
}
