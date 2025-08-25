using MarketWatch.Entities;

namespace MW.Price.Fetcher.Adapters
{
    public interface IDataFetcher
    {
        /// <summary>
        /// Fetches raw historical data for a given symbol.
        /// </summary>
        Task<IEnumerable<RawPriceData>> FetchHistoricalAsync(string symbol, Timeframe timeframe, int lookbackDays);
    }

    /// <summary>
    /// Represents a single raw bar from a data source.
    /// </summary>
    public class RawPriceData
    {
        public string Symbol { get; set; } = null!;
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public long Volume { get; set; }
        public DateTime Time { get; set; }
    }
}
