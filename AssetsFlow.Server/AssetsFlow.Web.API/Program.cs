using HsR.Infrastructure;
using HsR.Journal.DataContext;
using HsR.Journal.DataSeeder;
using HsR.Journal.Infrastructure;
using HsR.Web.API.Configurations;
using HsR.Web.API.Repositories;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Serilog;

LoggingConfiguration.ConfigureLogging();

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

bool isDev = builder.Environment.IsDevelopment() /*&& false*/;

#pragma warning disable CS8604 // Disable warning for possible null reference argument
string? connectionString = DbConnectionsWrapper.GetConnectionStringByEnv(isDev);
builder.Services.ConfigureTradingJournalDbContext(connectionString, isDev);

builder.ConfigureForEnvironment();

builder.Services.AddConfiguredControllers();
//builder.Services.AddCustomSwagger();
builder.Services.AddCustomAutoMapper();
builder.Services.AddRepositories();
builder.Services.AddCustomApiVersioning();

var app = builder.Build();

if (isDev)
    await app.Services.SeedDatabaseAsync();

Middleware.ConfigureMiddleware(app);

app.Run();


