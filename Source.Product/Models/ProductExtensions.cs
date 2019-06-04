using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Source.Database.Bases.Models;

namespace Source.Auth.Models
{
    public static class ProductExtensions {
        public static void EnsureSeedData(this ProductDbContext ctx)
        {
            if (ctx.AllMigrationsApplied())
            {

                // if (!ctx.Roles.Any())
                // {
                //     ctx.Roles.Add(new BaseRole() { Name = "SysAdmin", RoleName = "SysAdmin", NormalizedName="SYSADMIN", Description = "SysAdmin" });
                //     ctx.Roles.Add(new BaseRole() { Name = "SysUser", RoleName = "SysUser", NormalizedName = "SYSUSER", Description = "SysUser" });
                //     ctx.SaveChanges();
                // }
            }
        }
    }
}
