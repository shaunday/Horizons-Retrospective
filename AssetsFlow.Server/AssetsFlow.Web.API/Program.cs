using HsR.Infrastructure;
using HsR.Journal.DataContext;
using HsR.Journal.DataSeeder;
using HsR.Web.API.Configurations;
using HsR.Web.API.Repositories;
using Serilog;

LoggingConfiguration.ConfigureLogging();

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

DotNetEnv.Env.Load();
#pragma warning disable CS8604 // Disable warning for possible null reference argument
string? connectionString = Environment.GetEnvironmentVariable(TradingJournalContextFactory.DbConnectionString);
builder.Services.ConfigureTradingJournalDbContext(connectionString, isProduction:  !builder.Environment.IsDevelopment());

builder.ConfigureForEnvironment();

builder.Services.AddConfiguredControllers();
//builder.Services.AddCustomSwagger();
builder.Services.AddCustomAutoMapper();
builder.Services.AddRepositories();
builder.Services.AddCustomApiVersioning();

var app = builder.Build();

await app.Services.SeedDatabaseAsync();

Middleware.ConfigureMiddleware(app);

app.Run();


