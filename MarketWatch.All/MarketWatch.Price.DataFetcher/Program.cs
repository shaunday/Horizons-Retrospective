using MarketWatch.Data.Services;
using MarketWatch.Price.Repository;
using Serilog;

Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.File("logs/DataFetcher.log", rollingInterval: RollingInterval.Day)
            .CreateLogger();

Log.Information("Starting MarketWatch DataFetcher...");

using var dbContext = new MarketDbContext();

// ---------- Setup repository & service ----------
var securityRepository = new SecurityRepository(dbContext);
var securityService = new SecurityService(securityRepository);