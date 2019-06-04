using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.ComponentModel;
using Source.Payment.Interfaces;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Source.Payment.Models
{


    public class GenericRepository<T> : IRepository<T> where T : class
    {
        private readonly IDbContext _dbContext;
        public GenericRepository(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private DbSet<T> Set => _dbContext.Set<T>();

        public IQueryable<T> All()
        {
            return Set.AsQueryable();
        }

        public void Add(T entity)
        {
            Set.Add(entity);
        }

        public void AddRange(IEnumerable<T> entities)
        {
            Set.AddRange(entities);
        }

        public Task AddAsync(T entity)
        {
            return Set.AddAsync(entity);
        }

        public void Attach(T entity)
        {
            Set.Attach(entity);
        }

        public void Delete(T entity)
        {
            Set.Remove(entity);
        }
        public void DeleteRange(IEnumerable<T> entites)
        {
            Set.RemoveRange(entites);
        }

        public void Update(T entity)
        {
            Set.Update(entity);
        }

        public void UpdateRange(IEnumerable<T> entites)
        {
            Set.UpdateRange(entites);
        }


        public IQueryable<T> Find(Expression<Func<T, bool>> expression)
        {
            return Set.Where(expression);
        }

        public Task<T[]> FindAsync(Expression<Func<T, bool>> expression)
        {
            return Set.Where(expression).ToArrayAsync();
        }

        public IQueryable<T> Find<K>(Expression<Func<T, bool>> expression, Expression<Func<T, K>> orderExpression, bool isOrderByDesc)
        {
            var result = Set.AsQueryable();
            if (expression != null)
            {
                result = result.Where(expression);
            }
            if (orderExpression == null) return result;
            result = isOrderByDesc ? result.OrderByDescending(orderExpression) : result.OrderBy(orderExpression);
            return result;
        }

        public Task<T[]> FindAsync<K>(Expression<Func<T, bool>> expression, Expression<Func<T, K>> orderExpression, bool isOrderByDesc)
        {
            var result = Set.AsQueryable();
            if (expression != null)
            {
                result = result.Where(expression);
            }
            if (orderExpression == null) return result.ToArrayAsync();
            result = isOrderByDesc ? result.OrderByDescending(orderExpression) : result.OrderBy(orderExpression);
            return result.ToArrayAsync();
        }

        public int Save()
        {
            return _dbContext.SaveChanges();
        }

        public Task<int> SaveAsync()
        {
            return _dbContext.SaveChangesAsync();
        }

        public T Single(Expression<Func<T, bool>> where)
        {
            return Set.SingleOrDefault();
        }

        public Task<T> SingleAsync(Expression<Func<T, bool>> where)
        {
            return Set.SingleOrDefaultAsync();
        }
        public void ChangeState(T entity, EntityState entityState)
        {
            _dbContext.Entry(entity).State = entityState;
        }
        public Dictionary<string, string> GetHeaders()
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            var properties = typeof(T).GetProperties();
            foreach (var prop in properties)
            {
                var displayName = prop.GetCustomAttribute(typeof(DisplayNameAttribute), true) as DisplayNameAttribute;
                if (displayName == null)
                {
                    continue;
                }
                var fieldName = prop.Name;
                headers.Add(fieldName, displayName.DisplayName);
            }

            return headers;
        }
    }
}
