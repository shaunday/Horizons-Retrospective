using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;

namespace HsR.Journal.Infrastructure
{
    public static class DbConnectionsWrapper
    {
        private const string DevDbUser = "postgres";
        private const string DevDbPass= "meefi_"; //dev

        private const string SupabasePassVar = "SUPABASE_DB_PASS";
        private const string SupabaseIdVar = "SUPABASE_DB_ID";

        public static string GetConnectionStringByEnv(bool isDev = true)
        {
            Console.WriteLine("Current working dir: " + Environment.CurrentDirectory);

            if (isDev)
            {
                return $"Host=localhost;Port=5432;Database=HsR_Journal_Database;Username={DevDbUser};Password={DevDbPass};Include Error Detail=true";
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
