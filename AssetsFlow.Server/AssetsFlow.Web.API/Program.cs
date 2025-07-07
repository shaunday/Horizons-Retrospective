using AssetsFlowWeb.API.Configurations;
using DotNetEnv;
using HsR.Infrastructure;
using HsR.Journal.DataContext;
using HsR.Journal.DataSeeder;
using HsR.Journal.Infrastructure;
using HsR.UserService.Client.Extensions;
using Serilog;
using HsR.Common.AspNet;

Env.Load(Path.Combine(AppContext.BaseDirectory, ".env"));
Env.Load(Path.Combine(AppContext.BaseDirectory, ".env.AssetsFlow"));

var builder = WebApplication.CreateBuilder(args).ConfigureLogging();

Log.Information("Configuring for environment: {Environment}", builder.Environment.EnvironmentName);
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

bool isDev = builder.Environment.IsDevelopment();

builder.Services
    .AddConfiguredControllers()
    .AddCustomAutoMapper()
    .AddRepositories()
    .AddCustomApiVersioning()
    .AddConfigurationServices(builder.Configuration)
    .AddCacheServices(builder.Configuration)
    .AddUserServiceClient(builder.Configuration)
    .AddJwtAuthentication(builder.Configuration)
    .ConfigureTradingJournalDbContext(isDev)
    .ConfigureCors(isDev);

if (isDev)
{
    builder.Services.ApplyDevConfig();
}
else
{
    WebHostConfig.ConfigureUrls(builder.WebHost);
}

var app = builder.Build();

await app.EnsureDatabaseCreatedAsync<TradingJournalDataContext>();
if (app.Environment.IsDevelopment())
{
    await app.Services.FlushDbAndSeedDemoAsync();
}

Middleware.ConfigureMiddleware(app);

app.Run();


