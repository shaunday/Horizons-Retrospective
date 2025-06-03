using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;

namespace HsR.Journal.Infrastructure
{
    public static class DbConnectionsWrapper
    {
        private const string DbUserVar = "DB_USER";
        private const string DbPassVar = "DB_PASS";
        private const string SupabasePassVar = "SUPABASE_DB_PASS";
        private const string SupabaseIdVar = "SUPABASE_DB_ID";

        public static string GetConnectionStringByEnv(bool isDev = true)
        {
            Console.WriteLine("Current working dir: " + Environment.CurrentDirectory);

            if (isDev)
            {
                DotNetEnv.Env.Load();

                var dbUser = Environment.GetEnvironmentVariable(DbUserVar);
                var dbPass = Environment.GetEnvironmentVariable(DbPassVar);

                if (string.IsNullOrWhiteSpace(dbUser) || string.IsNullOrWhiteSpace(dbPass))
                {
                    throw new ApplicationException("Missing local DB credentials. Make sure DB_USER and DB_PASS are set.");
                }

                return $"Host=localhost;Port=5432;Database=HsR_Journal_Database;Username={dbUser};Password={dbPass};Include Error Detail=true";
            }
            else // Production
            {
                DotNetEnv.Env.Load(@"..\..\AssetsFlow.Web.API\.env");

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
                
                //ipv6 doesnt work here on local todo check on container
                //return $"Host=db.cavtnmvmhxbttxtgvyyt.supabase.co;Database=postgres;Username=postgres;Password={supabasePassword};SSL Mode=Require;Trust Server Certificate=true";
            }
        }
    }
}
