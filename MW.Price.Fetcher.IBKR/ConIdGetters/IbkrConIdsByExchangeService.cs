using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Serilog;

namespace MW.Price.Fetcher.IBKR.ConIdGetters
{
    public class IbkrConIdsByExchangeService
    {
        private readonly IbkrApiClient _api;
        private readonly ILogger _logger;
        private const string AllConidsEndpoint = "trsrv/all-conids";

        public IbkrConIdsByExchangeService(IbkrApiClient api, ILogger logger)
        {
            _api = api;
            _logger = logger;
        }

        /// <summary>
        /// Get all symbol–conid pairs available on a given exchange (stocks only)
        /// </summary>
        public async Task<List<(string Symbol, int Conid)>?> GetAllSymbolsAsync(string exchange)
        {
            try
            {
                var url = $"{AllConidsEndpoint}?exchange={exchange}";
                var json = await _api.GetAsync(url);

                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                var result = new List<(string Symbol, int Conid)>();

                foreach (var item in root.EnumerateArray())
                {
                    if (item.TryGetProperty("symbol", out var symbolProp) &&
                        item.TryGetProperty("conid", out var conidProp))
                    {
                        result.Add((symbolProp.GetString()!, conidProp.GetInt32()));
                    }
                }

                // Only log summary
                _logger.Information("Fetched {Count} symbols from {Exchange}", result.Count, exchange);
                Console.WriteLine($"✅ {exchange}: {result.Count} symbols");

                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to get symbols for exchange {Exchange}", exchange);
                Console.WriteLine($"❌ {exchange}: failed ({ex.Message})");
                return null;
            }
        }
    }
}
