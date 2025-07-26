using System;

namespace HsR.UserService.Infrastructure
{
    public static class UsersDbConnectionsWrapper
    {
        private const string DevDbUser = "postgres";
        private const string DevDbPass = "meefi_"; //dev

        private const string SupabasePassVar = "SUPABASE_DB_PASS";
        private const string SupabaseIdVar = "SUPABASE_DB_ID";

        public static string GetConnectionStringByEnv(bool isDev = true)
        {
            Console.WriteLine("Current working dir: " + Environment.CurrentDirectory);

            if (isDev)
            {
                return $"Host=localhost;Port=5432;Database=HsR_Users_Database;Username={DevDbUser};Password={DevDbPass};Include Error Detail=true";
            }
            else // Production
            {
                var supabasePassword = Environment.GetEnvironmentVariable(SupabasePassVar);
                var supabaseConnectionId = Environment.GetEnvironmentVariable(SupabaseIdVar);


                if (string.IsNullOrWhiteSpace(supabasePassword))
                {
                    throw new ApplicationException("Missing Supabase DB password. Set SUPABASE_DB_PASS environment variable.");
                }

                if (string.IsNullOrWhiteSpace(supabaseConnectionId))
                {
                    throw new ApplicationException("Missing Supabase DB User ID. Set SUPABASE_DB_ID environment variable.");
                }

                //ipv4 version
                return $"User Id=postgres.{supabaseConnectionId};Password={supabasePassword};Server=aws-0-eu-central-1.pooler.supabase.com;Port=5432;Database=postgres";

            }
        }
    }
} 