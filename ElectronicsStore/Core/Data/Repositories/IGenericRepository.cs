using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ElectronicsStore.Core.Data.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        T GetById(int id);
        IEnumerable<T> GetAll();
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
        void Add(T entity);
        void Update(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
        void SaveChanges();
    }
}
