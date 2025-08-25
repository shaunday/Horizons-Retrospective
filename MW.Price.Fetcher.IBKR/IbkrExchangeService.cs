using System;
using Serilog;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace MW.Price.Fetcher.IBKR
{
    public class IbkrExchangeService
    {
        private readonly IbkrApiClient _api;
        private readonly ILogger _logger;
        private const string AllConidsEndpoint = "trsrv/all-conids";

        public IbkrExchangeService(IbkrApiClient api, ILogger logger)
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

                _logger.Information("Fetched {Count} symbols from {Exchange}", result.Count, exchange);
                Console.WriteLine($"✅ {result.Count} symbols from {exchange}");

                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to get symbols for exchange {Exchange}", exchange);
                Console.WriteLine($"❌ Failed to get symbols for {exchange}: {ex.Message}");
                return null;
            }
        }
    }
}
