using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Source.Auth.Models
{
    public class BaseAuthDbContext : IdentityDbContext<BaseUser, BaseRole, Guid>
    {
        public BaseAuthDbContext(DbContextOptions<BaseAuthDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            // foreach (var entity in builder.Model.GetEntityTypes())
            // {
            //     entity.Relational().TableName = $"MOOC_{entity.ClrType.Name}";
            // }

            builder.Entity<BaseUser>(entity =>
            {
                entity.HasIndex(g => g.PhoneNumber).IsUnique();
                entity.ToTable(name: "Base_User", schema: "Security");
            });

            builder.Entity<BaseRole>(entity =>
            {
                entity.ToTable(name: "Base_Role", schema: "Security");

            });

            builder.Entity<IdentityUserClaim<Guid>>(entity =>
            {
                entity.ToTable("Base_UserClaim", "Security");
            });

            builder.Entity<IdentityUserLogin<Guid>>(entity =>
            {
                entity.ToTable("Base_UserLogin", "Security");
            });

            builder.Entity<IdentityRoleClaim<Guid>>(entity =>
            {
                entity.ToTable("Base_RoleClaim", "Security");
            });

            builder.Entity<IdentityUserRole<Guid>>(entity =>
            {
                entity.ToTable("Base_UserRole", "Security");
            });

            builder.Entity<IdentityUserToken<Guid>>(entity =>
            {
                entity.ToTable("Base_UserToken", "Security");
            });
        }
    }
}
