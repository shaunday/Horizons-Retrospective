using System;
using System.Collections.Generic;
using System.Text.Json;
using Serilog;
using System.Threading.Tasks;

namespace MW.Price.Fetcher.IBKR.ConIdGetters
{
    public class IbkrConIdBySymbolService
    {
        private const string SecdefEndpoint = "trsrv/secdef";

        private readonly IbkrApiClient _api;
        private readonly ILogger _logger;

        public IbkrConIdBySymbolService(IbkrApiClient api, ILogger logger)
        {
            _api = api;
            _logger = logger;
        }

        public async Task<Dictionary<string, JsonElement>?> ResolveSymbolAsync(string symbol)
        {
            try
            {
                var body = new { symbols = new[] { symbol } };
                var json = await _api.PostAsync(SecdefEndpoint, body); // use constant

                using var doc = JsonDocument.Parse(json);
                var rootArray = doc.RootElement;

                if (rootArray.GetArrayLength() == 0)
                {
                    _logger.Warning("No symbol resolution for {Symbol}", symbol);
                    Console.WriteLine($"⚠️ No resolution for {symbol}");
                    return null;
                }

                var first = rootArray[0];

                var dict = new Dictionary<string, JsonElement>();
                foreach (var prop in first.EnumerateObject())
                {
                    dict[prop.Name] = prop.Value; // keep raw JsonElement
                }

                _logger.Information("Resolved {Symbol} to {FieldCount} fields", symbol, dict.Count);
                Console.WriteLine($"✅ {symbol} → {dict.Count} fields");

                return dict;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to resolve symbol {Symbol}", symbol);
                Console.WriteLine($"❌ Failed to resolve {symbol}: {ex.Message}");
                return null;
            }
        }
    }
}
