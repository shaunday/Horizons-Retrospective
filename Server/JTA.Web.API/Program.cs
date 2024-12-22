using DayJT.Infrastructure;
using DayJT.Journal.DataContext.Services;
using DayJT.Journal.DataSeeder;
using DayJT.Journal.Repository.Services;
using DayJTrading.Web.API.Configurations;
using JTA.Web.API.Configurations;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using Serilog;
using System;

LoggingConfiguration.ConfigureLogging();

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();

builder.Configuration.AddEnvironmentVariables();
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

#pragma warning disable CS8604 // Disable warning for possible null reference argument
string? connectionString = builder.Configuration.GetConnectionString("JTA_Db_Key");
builder.Services.AddInfrastructureWithLogging(connectionString, builder.Environment);

builder.ConfigureForEnvironment();

builder.Services.AddConfiguredControllers();
builder.Services.AddCustomSwagger();
builder.Services.AddCustomAutoMapper();
builder.Services.AddScoped<ITradingJournalRepository, TradingJournalRepository>();
builder.Services.AddCustomApiVersioning();

var app = builder.Build();

await app.Services.SeedDatabaseAsync();

Middleware.ConfigureMiddleware(app);

app.Run();


