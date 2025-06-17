using AssetsFlowWeb.API.Configurations;
using HsR.Infrastructure;
using HsR.Journal.DataContext;
using HsR.Journal.DataSeeder;
using HsR.Journal.Infrastructure;
using HsR.Web.API.Configurations;
using HsR.Web.API.Repositories;
using HsR.Web.API.Services;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

bool isDev = builder.Environment.IsDevelopment();

LoggingConfiguration.ConfigureLogging(isDev);
builder.Host.UseSerilog();

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.Services.AddConfigurationServices(builder.Configuration);

#pragma warning disable CS8604 // Disable warning for possible null reference argument
string? connectionString = DbConnectionsWrapper.GetConnectionStringByEnv(isDev);
builder.Services.ConfigureTradingJournalDbContext(connectionString, isDev);

builder.ConfigureForEnvironment();

builder.Services.AddConfiguredControllers();
//builder.Services.AddCustomSwagger();
builder.Services.AddCustomAutoMapper();
builder.Services.AddRepositories();
builder.Services.AddCustomApiVersioning();
builder.Services.AddCacheServices();

var app = builder.Build();

if (isDev)
    await app.Services.SeedDatabaseAsync();

// Preload trades cache
using (var scope = app.Services.CreateScope())
{
    var cacheService = scope.ServiceProvider.GetRequiredService<ITradesCacheService>();
    cacheService.LoadCache(); //launch and forget
}

Middleware.ConfigureMiddleware(app);

app.Run();


