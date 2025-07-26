using System;

namespace HsR.UserService.Infrastructure
{
    public static class DbConnectionsWrapper
    {
        private const string DevDbUser = "postgres";
        private const string DevDbPass = "meefi_"; //dev

        public static string GetConnectionStringByEnv(bool isDev = true)
        {
            Console.WriteLine("Current working dir: " + Environment.CurrentDirectory);

            if (isDev)
            {
                return $"Host=localhost;Port=5432;Database=HsR_Users_Database;Username={DevDbUser};Password={DevDbPass};Include Error Detail=true";
            }
            else // Production
            {
                // For now, use the same dev connection string
                // TODO: Add .env file support for production
                return $"Host=localhost;Port=5432;Database=HsR_Users_Database;Username={DevDbUser};Password={DevDbPass};Include Error Detail=true";
            }
        }
    }
} 