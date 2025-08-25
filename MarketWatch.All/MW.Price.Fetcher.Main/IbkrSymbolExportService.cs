using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Serilog;

namespace MW.Price.Fetcher.IBKR
{
    public class IbkrSymbolExportService
    {
        private readonly IbkrExchangeService _exchangeService;
        private readonly ILogger _logger;

        public IbkrSymbolExportService(IbkrExchangeService exchangeService, ILogger logger)
        {
            _exchangeService = exchangeService;
            _logger = logger;
        }

        public async Task ExportSymbolsAsync(List<string> exchanges)
        {
            foreach (var exchange in exchanges)
            {
                try
                {
                    var symbols = await _exchangeService.GetAllSymbolsAsync(exchange);
                    if (symbols != null)
                    {
                        var fileName = $"{exchange}_symbols.json";
                        await File.WriteAllTextAsync(
                            fileName,
                            JsonSerializer.Serialize(symbols, new JsonSerializerOptions { WriteIndented = true })
                        );

                        _logger.Information("Saved {Count} symbols to {FileName}", symbols.Count, fileName);
                        Console.WriteLine($"✅ Saved {symbols.Count} symbols to {fileName}");
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Failed processing exchange {Exchange}", exchange);
                    Console.WriteLine($"❌ Failed processing exchange {exchange}: {ex.Message}");
                }
            }

            Console.WriteLine("Done fetching all exchanges.");
            _logger.Information("Done fetching all exchanges.");
        }
    }
}
