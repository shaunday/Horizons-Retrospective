using CarvedRock.Core;
using HsR.UserService.Data;
using HsR.UserService.Services;
using Microsoft.EntityFrameworkCore;

namespace HsR.UserService.Host.Configurations
{
    public static class Middleware
    {
        public static WebApplication ConfigureUserServicePipeline(this WebApplication app)
        {
            // Configure the HTTP request pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMiddleware<UserScopeMiddleware>();

            return app;
        }
    }
} 