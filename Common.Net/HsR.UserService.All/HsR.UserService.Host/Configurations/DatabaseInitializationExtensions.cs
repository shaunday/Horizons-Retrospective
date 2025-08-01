using HsR.UserService.Data;
using HsR.UserService.Services;
using Microsoft.EntityFrameworkCore;

namespace HsR.UserService.Host.Configurations
{
    public static class DatabaseInitializationExtensions
    {
        public static async Task EnsureUserServiceDatabaseAndUsersAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<UserDbContext>();
            await db.Database.MigrateAsync();  // Apply migrations on startup

            var userService = scope.ServiceProvider.GetService<IUserService>() as HsR.UserService.Services.UserService;
            if (userService == null)
            {
                throw new InvalidOperationException("IUserService not registered or cannot be resolved.");
            }

            await userService.EnsureDemoUserExistsAsync();
            await userService.EnsureAdminUserExistsAsync();
        }

    }
} 