using System;

namespace HsR.UserService.Infrastructure
{
    public static class UsersDbConnectionsWrapper
    {
        private const string SupabasePassVar = "SUPABASE_DB_ID";
        private const string SupabaseIdVar = "SUPABASE_DB_PASS";

        public static string GetConnectionStringByEnv(bool isDev = true)
        {
            Console.WriteLine("Current working dir: " + Environment.CurrentDirectory);

            if (isDev)
            {
                var pgDevPass = Environment.GetEnvironmentVariable("SUPA_DEV_PASS");
                string pgDevUser = "postgres";

                return $"Host=localhost;Port=5432;Database=HsR_Users_Database;Username={pgDevUser};Password={pgDevPass};Include Error Detail=true";
            }
            else // Production
            {
                var supabaseConnectionId = Environment.GetEnvironmentVariable(SupabaseIdVar);
                var supabasePassword = Environment.GetEnvironmentVariable(SupabasePassVar);

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