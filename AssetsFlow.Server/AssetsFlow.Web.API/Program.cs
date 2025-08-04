using AssetsFlowWeb.API.Configurations;
using DotNetEnv;
using HsR.Infrastructure;
using HsR.Journal.DataContext;
using HsR.Journal.DataSeeder;
using HsR.Journal.Seeder;
using HsR.UserService.Client.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;
using Serilog;

Env.Load(Path.Combine(AppContext.BaseDirectory, ".env.Global"));
Env.Load(Path.Combine(AppContext.BaseDirectory, ".env.AssetsFlow.Server"));

var builder = WebApplication.CreateBuilder(args).ConfigureLogging();

builder.AddServiceDefaults();
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

Log.Information("Configuring for environment: {Environment}", builder.Environment.EnvironmentName);
bool isDev = builder.Environment.IsDevelopment();

builder.Services
    .AddConfiguredControllers()
    .AddCustomAutoMapper()
    .AddRepositories()
    .AddCustomApiVersioning()
    .AddConfigurationServices(builder.Configuration)
    .AddCacheServices(builder.Configuration)
    .AddUserServiceClient()
    .AddJwtAuthentication(builder.Configuration)
    .ConfigureTradingJournalDbContext(isDev)
    .ConfigureCors(isDev)
    .RegisterDbSeeder();

if (isDev)
{
    IdentityModelEventSource.ShowPII = true;
}
else if (Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true")
{
    builder.WebHost.UseUrls("http://0.0.0.0:80");
}

var app = builder.Build();

app.MapDefaultEndpoints();

await app.InitDB(isDev);

Middleware.ConfigureMiddleware(app);

app.Run();


