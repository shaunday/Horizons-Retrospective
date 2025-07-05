using System;

namespace AssetsFlowWeb.API.Services
{
    public static class ExceptionMappingService
    {
        public static (int StatusCode, string Message) MapToStatusCode(Exception ex)
        {
            return ex switch
            {
                ArgumentException argEx => (400, argEx.Message),
                InvalidOperationException invOpEx => (404, invOpEx.Message),
                _ => (500, "Internal server error")
            };
        }
    }
} 