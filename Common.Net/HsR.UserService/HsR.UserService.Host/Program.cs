using HsR.UserService.Host.Configurations;
using HsR.UserService.Services;
using HsR.UserService.Data;
using HsR.UserService.Infrastructure;
using HsR.Common.AspNet;
using Serilog;

// Load shared environment variables
DotNetEnv.Env.Load(".env.AssetsFlow");

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