using MarketWatch.Data.Services;
using MarketWatch.Price.Repository;
using MW.Price.Fetcher.IBKR;
using MW.Price.Fetcher.IBKR.ConIdGetters;
using Serilog;
using System.Text.Json;

Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.File("logs/DataFetcher.log", rollingInterval: RollingInterval.Day)
            .CreateLogger();

Log.Information("Starting MarketWatch DataFetcher...");

var apiClient = new IbkrApiClient();


// ---------- Symbols Fetcher ----------
Console.WriteLine("Symbols fetch complete. Do you want to continue? (y/n)");
var input = Console.ReadLine()?.Trim().ToLower();

if (input == "y")
{
    Log.Information("Fetching Symbols by exchange...");
    var exchangeService = new IbkrConIdsByExchangeService(apiClient, Log.Logger);
    var exportService = new IbkSymbolsExportService(exchangeService, Log.Logger);

    var exchanges = new List<string> { "NASDAQ", /*"NYSE", "TSX"*/ };
    await exportService.ExportSymbolsAsync(exchanges);
}

// ----------




// ---------- Setup repository & service ----------
using var dbContext = new MarketDbContext();

var securityRepository = new SecurityRepository(dbContext);
var securityService = new SecurityService(securityRepository);

// ----------