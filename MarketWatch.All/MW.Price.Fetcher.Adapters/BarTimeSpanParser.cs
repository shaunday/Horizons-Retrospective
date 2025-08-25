using MarketWatch.Entities;

namespace MW.Price.Fetcher.Adapters
{
    public class BarTimeSpanParser
    {
        public TimeSpan Parse(string bar) => bar switch
        {
            "1m" => TimeSpan.FromMinutes(1),
            "5m" => TimeSpan.FromMinutes(5),
            "15m" => TimeSpan.FromMinutes(15),
            "1h" => TimeSpan.FromHours(1),
            "1d" => TimeSpan.FromDays(1),
            "1w" => TimeSpan.FromDays(7),
            _ => throw new ArgumentException($"Unknown bar: {bar}")
        };
    }
}
