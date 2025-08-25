using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using MW.Price.Fetcher.IBKR.ConIdGetters;
using Serilog;

namespace MW.Price.Fetcher.IBKR
{
    public class IbkSymbolsExportService
    {
        private readonly IbkrConIdsByExchangeService _exchangeService;
        private readonly ILogger _logger;
        private readonly int _delayMilliseconds;

        public IbkSymbolsExportService(IbkrConIdsByExchangeService exchangeService, ILogger logger, int delayMilliseconds = 500)
        {
            _exchangeService = exchangeService;
            _logger = logger;
            _delayMilliseconds = delayMilliseconds; // default delay between requests
        }

        public async Task ExportSymbolsAsync(List<string> exchanges)
        {
            var folder = Path.Combine(AppContext.BaseDirectory, "..", "Data");
            Directory.CreateDirectory(folder); // ensure folder exists

            var dateSuffix = DateTime.UtcNow.ToString("yyyyMMdd");

            foreach (var exchange in exchanges)
            {
                try
                {
                    _logger.Information("Fetching data for exchange =", exchange);
                    var symbols = await _exchangeService.GetAllSymbolsAsync(exchange);
                    if (symbols != null)
                    {
                        var fileName = Path.Combine(folder, $"{exchange}_symbols_{dateSuffix}.json");

                        using var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None);
                        using var writer = new Utf8JsonWriter(fs, new JsonWriterOptions { Indented = true });

                        JsonSerializer.Serialize(writer, symbols);

                        _logger.Information("Saved {Count} symbols to {FileName}", symbols.Count, fileName);
                        Console.WriteLine($"✅ Saved {symbols.Count} symbols to {fileName}");
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Failed processing exchange {Exchange}", exchange);
                    Console.WriteLine($"❌ Failed processing exchange {exchange}: {ex.Message}");
                }

                // delay to respect IBKR pacing limits
                await Task.Delay(_delayMilliseconds);
            }

            Console.WriteLine("Done fetching all exchanges.");
            _logger.Information("Done fetching all exchanges.");
        }
    }
}
