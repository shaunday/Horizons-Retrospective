using DotNetEnv;
using HsR.Common.AspNet;
using HsR.UserService.Data;
using HsR.UserService.Host.Configurations;
using HsR.UserService.Infrastructure;
using HsR.UserService.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Serilog;

EnvLoader.LoadEnvFiles(
[
    Path.Combine(AppContext.BaseDirectory, ".env.UserService"),
    Path.Combine(AppContext.BaseDirectory, ".env.Global")
]);

var builder = WebApplication.CreateBuilder(args)
    .ConfigureLogging()
    .ConfigureUserServiceHost();

builder.AddServiceDefaults();
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

Log.Information("Configuring for environment: {Environment}", builder.Environment.EnvironmentName);
bool isDev = builder.Environment.IsDevelopment();

string connectionString = builder.GetUserServiceConnectionString();
builder.Services.AddUserServiceAllServices(connectionString);

var app = builder.Build();

app.MapDefaultEndpoints();

app.ConfigureUserServicePipeline();
app.MapGrpcService<UserGrpcService>();
await app.EnsureUserServiceDatabaseAndUsersAsync(isDev);

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