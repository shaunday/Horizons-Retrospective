using HsR.UserService.Data;
using HsR.UserService.Services;
using HsR.Common.AspNet;
using Microsoft.EntityFrameworkCore;

namespace HsR.UserService.Host.Configurations
{
    public static class DatabaseInitializationExtensions
    {
        public static async Task EnsureUserServiceDatabaseAndUsersAsync(this WebApplication app)
        {
            await app.EnsureDatabaseCreatedAsync<UserDbContext>();
            await app.EnsureDemoUserCreatedAsync();
            await app.EnsureAdminUserCreatedAsync();
        }

        private static async Task EnsureDemoUserCreatedAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var userService = scope.ServiceProvider.GetRequiredService<IUserService>() as HsR.UserService.Services.UserService;
            if (userService != null)
            {
                await userService.EnsureDemoUserExistsAsync();
            }
        }

        private static async Task EnsureAdminUserCreatedAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var userService = scope.ServiceProvider.GetRequiredService<IUserService>() as HsR.UserService.Services.UserService;
            if (userService != null)
            {
                await userService.EnsureAdminUserExistsAsync();
            }
        }
    }
} 