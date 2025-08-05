using HsR.UserService.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HsR.UserService.Infrastructure;

public class UsersContextFactory : IDesignTimeDbContextFactory<UserDbContext>
{
    public UserDbContext CreateDbContext(string[] args)
    {
        var connectionString = UsersDbConnectionsWrapper.GetConnectionStringByEnv(false); //used for migration scripts, so set to production for now
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new ApplicationException($"Please set  environment variables");
        }

        var optionsBuilder = new DbContextOptionsBuilder<UserDbContext>();
        var migrationsAssembly = typeof(UserDbContext).Assembly.GetName().Name;

        optionsBuilder.UseNpgsql(connectionString, sqlOptions =>
        {
            sqlOptions.MigrationsAssembly(migrationsAssembly);
        });

        return new UserDbContext(optionsBuilder.Options);
    }
}