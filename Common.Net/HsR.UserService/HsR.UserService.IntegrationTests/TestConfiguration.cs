using Microsoft.Extensions.Configuration;

namespace HsR.UserService.IntegrationTests
{
    public static class TestConfiguration
    {
        private static readonly IConfiguration _configuration;

        static TestConfiguration()
        {
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true)
                .AddEnvironmentVariables()
                .Build();
        }

        public static string GrpcServiceUrl => 
            _configuration["GrpcService:BaseUrl"] ?? 
            Environment.GetEnvironmentVariable("GRPC_SERVICE_URL") ?? 
            "https://localhost:7001";

        public static string DefaultPassword => 
            _configuration["TestSettings:DefaultPassword"] ?? 
            Environment.GetEnvironmentVariable("TEST_DEFAULT_PASSWORD") ?? 
            "test1234";

        public static string NewPassword => 
            _configuration["TestSettings:NewPassword"] ?? 
            Environment.GetEnvironmentVariable("TEST_NEW_PASSWORD") ?? 
            "newpass1234";
    }
} 