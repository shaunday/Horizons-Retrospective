using HsR.UserService.Data;
using HsR.UserService.Entities;
using HsR.UserService.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HsR.UserService.Host.Configurations
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddUserServiceDbContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<UserDbContext>(options =>
                options.UseNpgsql(connectionString));
            return services;
        }

        public static IServiceCollection AddUserServiceIdentity(this IServiceCollection services)
        {
            services.AddIdentity<User, IdentityRole<Guid>>(options =>
            {
                options.Password.RequiredLength = 4;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = false;
            })
            .AddEntityFrameworkStores<UserDbContext>()
            .AddDefaultTokenProviders();
            return services;
        }

        public static IServiceCollection AddUserServiceCoreServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, HsR.UserService.Services.UserService>();
            services.AddScoped<UserGrpcService>();
            return services;
        }

        public static IServiceCollection AddUserServiceAllServices(this IServiceCollection services, string connectionString)
        {
            services.AddGrpc(); // Grpc doesn't return IServiceCollection

            return services
                .AddUserServiceDbContext(connectionString)
                .AddUserServiceIdentity()
                .AddAuthorization()
                .AddUserServiceCoreServices();
        }

    }
} 