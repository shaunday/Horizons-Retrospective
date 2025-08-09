namespace HsR.UserService.Contracts
{
    public static class UserServiceEx
    {
        public static string GetUserServiceUrl()
        {
            string baseUrl;
            if (Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true")
            {
                baseUrl = "http://userservice:80";
            }
            else
            {
                var userServicePort = Environment.GetEnvironmentVariable("USER_SERVICE_PORT") ?? "7001";
                baseUrl = $"https://localhost:{userServicePort}";
            }

            return baseUrl;
        }
    }
}
