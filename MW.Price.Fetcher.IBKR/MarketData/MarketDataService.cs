using Microsoft.Extensions.Logging;
using MW.Price.Fetcher.IBKR.MarketData;
using MW.Price.Fetcher.Adapters;
using Polly;
using Polly.RateLimit;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace IBKRApi.Services
{
    public class HistoricalMarketDataService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<HistoricalMarketDataService> _logger;
        private readonly BarTimeSpanParser _barParser;
        private readonly AsyncRateLimitPolicy _throttlePolicy;

        private const int MaxConcurrentRequests = 5;
        private const int MaxRequestsPerSecond = 6; // safe max
        private readonly SemaphoreSlim _semaphore = new(MaxConcurrentRequests);
        private static readonly string HistoricalDataEndpoint = "/iserver/marketdata/history";

        public HistoricalMarketDataService(
            HttpClient httpClient,
            ILogger<HistoricalMarketDataService> logger,
            BarTimeSpanParser barParser)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _barParser = barParser ?? throw new ArgumentNullException(nameof(barParser));

            _throttlePolicy = Policy.RateLimitAsync(MaxRequestsPerSecond, TimeSpan.FromSeconds(1));
        }

        public async Task<List<JsonElement>> GetHistoricalDataForConidAsync(
             string conid,
             string exchange,
             string period,
             string bar,
             DateTime from,
             bool outsideRth = true,
             CancellationToken token = default)
        {
            var ranges = MarketDataRequestSplitter.SplitByMaxPoints(from, DateTime.UtcNow, _barParser.Parse(bar), 1000);
            var mergedElements = new List<JsonElement>();

            foreach (var range in ranges)
            {
                try
                {
                    await _semaphore.WaitAsync(token);

                    var url = $"{HistoricalDataEndpoint}?" +
                              $"conid={conid}" +
                              $"&exchange={exchange}" +
                              $"&period={period}" +
                              $"&bar={bar}" +
                              $"&startTime={range.from:yyyyMMdd-HH:mm:ss}" +
                              $"&outsideRth={outsideRth.ToString().ToLower()}";

                    _logger.LogDebug("Requesting historical data: {Url}", url);

                    // throttle via Polly
                    var response = await _throttlePolicy.ExecuteAsync(async ct =>
                    {
                        return await _httpClient.GetAsync(url, ct);
                    }, token);

                    response.EnsureSuccessStatusCode();

                    var json = await JsonSerializer.DeserializeAsync<JsonElement>(
                        await response.Content.ReadAsStreamAsync(token),
                        cancellationToken: token
                    );

                    if (json.ValueKind == JsonValueKind.Array)
                        mergedElements.AddRange(json.EnumerateArray());
                    else
                        mergedElements.Add(json);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error fetching historical data for {Conid} in range {From} - {To}", conid, range.from, range.to);
                }
                finally
                {
                    _semaphore.Release();
                }
            }

            _logger.LogInformation("Completed fetching historical data for {Conid}. Total responses: {Count}", conid, mergedElements.Count);
            return mergedElements;
        }
    }
}