using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;

namespace HsR.Journal.Infrastructure
{
    public static class DbConnectionInfo
    {
        private const string DbUserVar = "DB_USER";
        private const string DbPassVar = "DB_PASS";
        private const string SupabasePassVar = "SUPABASE_DB_PASS";

        public static string GetConnectionStringByEnv(bool isDev = true)
        {
            DotNetEnv.Env.Load();

            if (isDev)
            {
                var dbUser = Environment.GetEnvironmentVariable(DbUserVar);
                var dbPass = Environment.GetEnvironmentVariable(DbPassVar);

                return $"Host=localhost;Port=5432;Database=HsR_Journal_Database;Username={dbUser};Password={dbPass};Include Error Detail=true";
            }
            else // Production
            {
                var supabasePassword = Environment.GetEnvironmentVariable(SupabasePassVar);
                //ipv4 version
                return $"User Id=postgres.cavtnmvmhxbttxtgvyyt;Password={supabasePassword};Server=aws-0-eu-central-1.pooler.supabase.com;Port=5432;Database=postgres";
                
                //ipv6 doesnt work here on local todo check on container
                //return $"Host=db.cavtnmvmhxbttxtgvyyt.supabase.co;Database=postgres;Username=postgres;Password={supabasePassword};SSL Mode=Require;Trust Server Certificate=true";
            }
        }
    }
}
