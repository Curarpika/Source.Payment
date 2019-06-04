using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Source.Payment.Interfaces;

namespace Source.Payment.Models
{  
    public class PaymentDbContext : DbContext,IDbContext
    {  
        private string _user;
        public PaymentDbContext(DbContextOptions<PaymentDbContext> options, IHttpContextAccessor httpContext) : base(options)  
        {  
            _user = httpContext?.HttpContext?.User.Identity.Name ?? httpContext?.HttpContext?.Connection.RemoteIpAddress.ToString();
        }  
  
        public override int SaveChanges()
        {
            var modifiedEntries = ChangeTracker.Entries()
              .Where(x => x.Entity is IAuditEntity
                  && (x.State == EntityState.Added || x.State == EntityState.Modified || x.State == EntityState.Deleted));

            foreach (var entry in modifiedEntries)
            {
                IAuditEntity entity = entry.Entity as IAuditEntity;
                if (entity != null)
                {
                    DateTime now = DateTime.Now;

                    if (entry.State == EntityState.Added)
                    {
                        entity.CreatedTime = now;
                        entity.CreatedBy = _user;
                    }
                    else
                    {
                        base.Entry(entity).Property(x => x.CreatedTime).IsModified = false;
                    }

                    entity.UpdatedTime = now;
                    entity.UpdatedBy = _user;
                }
            }
            return base.SaveChanges();
        }

        DbSet<PaymentOrder> PaymentOrders { get; set; }  
    }  
}  