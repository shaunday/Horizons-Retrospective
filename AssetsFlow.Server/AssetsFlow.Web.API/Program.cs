using AssetsFlowWeb.API.Configurations;
using DotNetEnv;
using HsR.Infrastructure;
using HsR.Web.API.Configurations;

Env.Load(Path.Combine(AppContext.BaseDirectory, ".env"));
Env.Load(Path.Combine(AppContext.BaseDirectory, ".env.AssetsFlow"));

var builder = WebApplication.CreateBuilder(args)
    .ConfigureAssetsFlowHost();

string connectionString = builder.GetAssetsFlowConnectionString();
bool isDev = builder.Environment.IsDevelopment();

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.Services.ConfigureTradingJournalDbContext(connectionString, isDev);
builder.ConfigureCorsAndEnvironment();

builder.Services.AddConfiguredControllers();
//builder.Services.AddCustomSwagger();
builder.Services.AddCustomAutoMapper();
builder.Services.AddRepositories();
builder.Services.AddCustomApiVersioning();
builder.Services.AddAssetsFlowServices(builder.Configuration, connectionString, isDev, builder.Environment);

var app = builder.Build();

await app.EnsureAssetsFlowDatabaseAndSeedAsync();
Middleware.ConfigureMiddleware(app);

app.Run();


