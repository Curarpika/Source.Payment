using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Source.WebAPI.Interfaces
{
    using Microsoft.EntityFrameworkCore;

    public interface IUnitOfWork:IDisposable
    {
        void ChangeObjectState<TEntity>(TEntity entity, EntityState entityState) where TEntity : class;

        int Save();

        Task<int> SaveAsync();
    }
}
