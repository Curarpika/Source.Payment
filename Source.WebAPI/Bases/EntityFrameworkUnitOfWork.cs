using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Source.WebAPI.Interfaces;

namespace Source.WebAPI.Bases
{
    public sealed class EntityFrameworkUnitOfWork : IUnitOfWork
    {
        //private readonly BusinessDbContext dbContext;
        public IDbContext dbContext { get; set; }

        public EntityFrameworkUnitOfWork(IDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Dispose()
        {
            this.dbContext?.Dispose();
            GC.SuppressFinalize(this);
        }

        public void ChangeObjectState<TEntity>(TEntity entity, EntityState entityState) where TEntity : class
        {
            this.dbContext.Entry(entity).State = entityState;
        }

        public int Save()
        {
            return this.dbContext.SaveChanges();
        }

        public Task<int> SaveAsync()
        {
            return this.dbContext.SaveChangesAsync();
        }
    }
}