using JTA.Infrastructure;
using JTA.Journal.DataContext;
using JTA.Journal.DataSeeder;
using JTA.Web.API.Configurations;
using JTA.Web.API.Repositories;
using Serilog;

LoggingConfiguration.ConfigureLogging();

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();

builder.Configuration.AddEnvironmentVariables();
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

#pragma warning disable CS8604 // Disable warning for possible null reference argument
string? connectionString = builder.Configuration.GetConnectionString("HsRJ_Db_Key");
builder.Services.AddInfrastructureWithLogging(connectionString, builder.Environment);

builder.ConfigureForEnvironment();

builder.Services.AddConfiguredControllers();
builder.Services.AddCustomSwagger();
builder.Services.AddCustomAutoMapper();
builder.Services.AddRepositories();
builder.Services.AddCustomApiVersioning();

var app = builder.Build();

await app.Services.SeedDatabaseAsync();

Middleware.ConfigureMiddleware(app);

app.Run();


