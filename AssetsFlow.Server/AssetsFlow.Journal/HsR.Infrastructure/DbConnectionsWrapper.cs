using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.IO;

namespace HsR.Journal.Infrastructure
{
    public static class DbConnectionsWrapper
    {
        private const string SupabaseIdVar = "SUPABASE_DB_ID";
        private const string SupabasePassVar = "SUPABASE_DB_PASS";

        public static string GetConnectionStringByEnv(bool isDev = true)
        {
            Console.WriteLine("Current working dir: " + Environment.CurrentDirectory);
            var supabasePassword = Environment.GetEnvironmentVariable(SupabasePassVar);

            if (isDev)
            {
                string DevDbUser = "postgres";
                return $"Host=localhost;Port=5432;Database=HsR_Journal_Database;Username={DevDbUser};Password={supabasePassword};Include Error Detail=true";
            }
            else // Production
            {
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
