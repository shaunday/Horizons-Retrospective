using HsR.UserService.Data;
using HsR.UserService.Entities;
using HsR.UserService.Services;
using HsR.UserService.Host.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog using extender
LoggingConfiguration.ConfigureLogging(builder.Environment.IsDevelopment());
builder.Host.UseSerilog();

// Add services to the container
// Removed: builder.Services.AddControllers();
// Removed: builder.Services.AddEndpointsApiExplorer();
// Removed: builder.Services.AddCors();

// Configure database
builder.Services.AddUserServiceDbContext(builder.Configuration);

// Configure Identity (only require minimum length 4 for password)
builder.Services.AddUserServiceIdentity();

// Register core services
builder.Services.AddScoped<UserService>();

// Removed: CORS configuration

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
// Removed: app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

// Removed: app.MapControllers();

// Ensure database is created
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<UserDbContext>();
    context.Database.EnsureCreated();
}

try
{
    Log.Information("Starting HsR User Service Host");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
} 