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
                baseUrl = $"https://localhost:7001";
            }

            return baseUrl;
        }
    }
}
