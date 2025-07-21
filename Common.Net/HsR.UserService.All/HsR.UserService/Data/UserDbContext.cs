using HsR.UserService.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HsR.UserService.Data
{
    public class UserDbContext : IdentityDbContext<User, Microsoft.AspNetCore.Identity.IdentityRole<Guid>, Guid>
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure User entity
            builder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasIndex(e => e.UserName).IsUnique();
            });

            // Configure Identity tables
            builder.Entity<Microsoft.AspNetCore.Identity.IdentityRole<Guid>>(entity =>
            {
                entity.ToTable("Roles");
            });

            builder.Entity<Microsoft.AspNetCore.Identity.IdentityUserRole<Guid>>(entity =>
            {
                entity.ToTable("UserRoles");
            });

            builder.Entity<Microsoft.AspNetCore.Identity.IdentityUserClaim<Guid>>(entity =>
            {
                entity.ToTable("UserClaims");
            });

            builder.Entity<Microsoft.AspNetCore.Identity.IdentityUserLogin<Guid>>(entity =>
            {
                entity.ToTable("UserLogins");
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });
            });

            builder.Entity<Microsoft.AspNetCore.Identity.IdentityUserToken<Guid>>(entity =>
            {
                entity.ToTable("UserTokens");
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });
            });

            builder.Entity<Microsoft.AspNetCore.Identity.IdentityRoleClaim<Guid>>(entity =>
            {
                entity.ToTable("RoleClaims");
            });
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken).ConfigureAwait(false);
        }
    }
} 