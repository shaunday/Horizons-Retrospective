using AssetsFlowWeb.API.Configurations;
using DotNetEnv;
using HsR.Infrastructure;
using HsR.Journal.Infrastructure;
using HsR.Web.API.Configurations;
using Serilog;
using HsR.UserService.Client.Extensions;

Env.Load(Path.Combine(AppContext.BaseDirectory, ".env"));
Env.Load(Path.Combine(AppContext.BaseDirectory, ".env.AssetsFlow"));

var builder = WebApplication.CreateBuilder(args).ConfigureAssetsFlowLogging();

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
    .AddUserServiceClient(builder.Configuration, builder.Environment)
    .AddJwtAuthentication(builder.Configuration);

string connectionString = DbConnectionsWrapper.GetConnectionStringByEnv(isDev);
builder.Services.ConfigureTradingJournalDbContext(connectionString, isDev);
builder.ConfigureCorsAndEnvironment();

var app = builder.Build();

await app.EnsureAssetsFlowDatabaseAndSeedAsync();
Middleware.ConfigureMiddleware(app);

app.Run();


