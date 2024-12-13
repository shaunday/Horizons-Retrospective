using DayJT.Configurations;
using DayJT.Journal.DataContext.Services;
using DayJT.Journal.DataSeeder;
using DayJT.Journal.Repository.Services;
using DayJTrading.Web.API.Configurations;
using Serilog;

LoggingConfiguration.ConfigureLogging();

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.Configuration.AddEnvironmentVariables();

builder.Services.AddConfiguredControllers();
builder.Services.AddTradingJournalDbContext(builder.Configuration, builder.Environment);
builder.Services.AddCustomSwagger();
builder.Services.AddCustomAutoMapper();
builder.Services.AddScoped<ITradingJournalRepository, TradingJournalRepository>();
builder.Services.AddCustomApiVersioning();

var app = builder.Build();

await app.Services.SeedDatabaseAsync();

Middleware.ConfigureMiddleware(app);

app.Run();


