using MarketWatch.Data.Services;
using MarketWatch.Price.Repository;
using MW.Price.Fetcher.IBKR;
using Serilog;
using System.Text.Json;

Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.File("logs/DataFetcher.log", rollingInterval: RollingInterval.Day)
            .CreateLogger();

Log.Information("Starting MarketWatch DataFetcher...");


// Exchanges to process
var exchanges = new List<string> { "NASDAQ", /*"NYSE", "TSX"*/ };


var apiClient = new IbkrApiClient();
var exchangeService = new IbkrExchangeService(apiClient, Log.Logger);
var exportService = new IbkrSymbolExportService(exchangeService, Log.Logger);

await exportService.ExportSymbolsAsync(exchanges);

using var dbContext = new MarketDbContext();

// ---------- Setup repository & service ----------
var securityRepository = new SecurityRepository(dbContext);
var securityService = new SecurityService(securityRepository);