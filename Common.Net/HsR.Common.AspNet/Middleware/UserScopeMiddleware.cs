using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using System.Security.Claims;

namespace HsR.Middleware
{
    public class UserScopeMiddleware(RequestDelegate next, ILogger<UserScopeMiddleware> logger)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<UserScopeMiddleware> _logger = logger;

        public async Task InvokeAsync(HttpContext context)
        {
            
            var userId = "unknown";
            //var userName = "unknown"; //todo

            if (context.User.Identity?.IsAuthenticated == true)
            {
                userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "unknown";
                //userName = context.User.Identity?.Name ?? "unknown";
            
                using (_logger.BeginScope(new Dictionary<string, object>
                {
                    ["UserId"] = userId,
                    //["UserName"] = userName
                }))
                {
                    await _next(context);
                }
            }
            else
            {
                await _next(context);
            }
        }

    }
}
