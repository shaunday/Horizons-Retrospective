using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HsR.Journal.Infrastructure
{
    public static class DbConnectionInfo
    {
        private static string Db_User_String = "DB_USER";
        private static string Db_Pass_String = "DB_PASS";

        public static string GetConnectionStringByEnv(bool isDev = true)
        {
            DotNetEnv.Env.Load();

            var dbUser = Environment.GetEnvironmentVariable(Db_User_String);
            var dbPass = Environment.GetEnvironmentVariable(Db_Pass_String);

            string connectionString;
            if (isDev)
            {
                connectionString = $"Host=localhost;Port=5432;Database=HsR_Journal_Database;Username={dbUser};Password={dbPass};Include Error Detail=true";
            }
            else // Production
            {
                connectionString = $"Host=db.supabase.co;Port=5432;Database=HsR_Journal_Prod;Username={dbUser};Password={dbPass};Include Error Detail=false;SSL Mode=Require";
            }

            return connectionString;
        }
    }
}
