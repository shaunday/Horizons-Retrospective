using HsR.UserService.Host.Configurations;
using HsR.UserService.Services;
using HsR.UserService.Data;
using HsR.UserService.Infrastructure;
using HsR.Common.AspNet;
using Serilog;
using HsR.UserService.Contracts;
using HsR.UserService.Models;
using DotNetEnv;

DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
LoggingConfiguration.ConfigureLogging(builder.Environment.IsDevelopment());
builder.Host.UseSerilog();

// Get database connection string based on environment
bool isDev = builder.Environment.IsDevelopment();
string? connectionString = DbConnectionsWrapper.GetConnectionStringByEnv(isDev);

// Add all services
builder.Services.AddGrpc();
builder.Services.AddUserServiceAllServices(builder.Configuration, connectionString);

var app = builder.Build();

// Configure the application
app.ConfigureUserServicePipeline();
app.MapGrpcService<UserGrpcService>();

await app.EnsureDatabaseCreatedAsync<UserDbContext>();
await EnsureDemoUserCreatedAsync(app);
await EnsureAdminUserCreatedAsync(app);

try
{
    Log.Information("Starting HsR User Service Host with gRPC");
    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

// Helper method for demo user creation
static async Task EnsureDemoUserCreatedAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var userService = scope.ServiceProvider.GetRequiredService<IUserService>() as UserService;
    if (userService != null)
    {
        await userService.EnsureDemoUserExistsAsync();
    }
}

static async Task EnsureAdminUserCreatedAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var userService = scope.ServiceProvider.GetRequiredService<IUserService>() as UserService;
    if (userService != null)
    {
        await userService.EnsureAdminUserExistsAsync();
    }
} 