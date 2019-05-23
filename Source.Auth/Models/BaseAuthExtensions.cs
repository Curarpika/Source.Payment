using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Source.Auth.Models
{
    public static class MOOCExtensions {
        public static void EnsureSeedData(this BaseAuthDbContext ctx)
        {
            if (ctx.AllMigrationsApplied())
            {

                if (!ctx.Roles.Any())
                {
                    ctx.Roles.Add(new BaseRole() { Name = "SysAdmin", RoleName = "SysAdmin", NormalizedName="SYSADMIN", Description = "SysAdmin" });
                    ctx.Roles.Add(new BaseRole() { Name = "SysUser", RoleName = "SysUser", NormalizedName = "SYSUSER", Description = "SysUser" });
                    ctx.SaveChanges();
                }
            }
        }
    }
}
