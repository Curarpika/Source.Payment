using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Source.Product.Interfaces
{
    public interface IRepository<T> where T: class
    {
        IQueryable<T> All();
        void Add(T entity);
        void AddRange(IEnumerable<T> entities);

        Task AddAsync(T entity);

        void Attach(T entity);

        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entites);
        void Update(T entity);
        void UpdateRange(IEnumerable<T> entites);

        IQueryable<T> Find(Expression<Func<T, bool>> expression);

            Task<T[]> FindAsync(Expression<Func<T, bool>> expression);

        IQueryable<T> Find<K>(Expression<Func<T, bool>> expression, Expression<Func<T, K>> orderExpression, bool isOrderByDesc);

        Task<T[]> FindAsync<K>(Expression<Func<T, bool>> expression, Expression<Func<T, K>> orderExpression, bool isOrderByDesc);

        int Save();

        Task<int> SaveAsync();

        T Single(Expression<Func<T, bool>> where);

        Task<T> SingleAsync(Expression<Func<T, bool>> where);
        void ChangeState(T entity, EntityState entityState);
        Dictionary<string, string> GetHeaders();

    }
}
