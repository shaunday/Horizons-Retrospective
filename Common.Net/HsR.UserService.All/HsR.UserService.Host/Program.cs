using DotNetEnv;
using HsR.Common.AspNet;
using HsR.UserService.Data;
using HsR.UserService.Host.Configurations;
using HsR.UserService.Infrastructure;
using HsR.UserService.Services;
using Serilog;

// Load  environment variables
Env.Load(Path.Combine(AppContext.BaseDirectory, ".env.UserService"));
Env.Load(Path.Combine(AppContext.BaseDirectory, ".env.Global"));

var builder = WebApplication.CreateBuilder(args)
    .ConfigureUserServiceHost();

string connectionString = builder.GetUserServiceConnectionString();
builder.Services.AddGrpc();
builder.Services.AddUserServiceAllServices(builder.Configuration, connectionString);

var app = builder.Build();

app.ConfigureUserServicePipeline();
app.MapGrpcService<UserGrpcService>();
await app.EnsureUserServiceDatabaseAndUsersAsync();

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