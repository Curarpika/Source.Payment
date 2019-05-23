using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Source.Payment.Models;

namespace Source.Auth.Models
{
    public static class PaymentExtensions {
        public static void EnsureSeedData(this PaymentDbContext ctx)
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
