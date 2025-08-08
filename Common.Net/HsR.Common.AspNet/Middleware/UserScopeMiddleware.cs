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
            if (context.User.Identity?.IsAuthenticated == true)
            {
                var userId = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "unknown";
                var userName = context.User.Identity?.Name ?? "unknown";

                using (_logger.BeginScope(new Dictionary<string, object>
                {
                    ["UserId"] = userId,
                    //["UserName"] = userName //todo
                }))
                {
                    await _next(context);
                }
            }
            else
            {
                using (LogContext.PushProperty("UserId", "unknown"))
                //using (LogContext.PushProperty("UserName", "unknown")) //todo
                {
                    await _next(context);
                }
            }
        }
    }
}
