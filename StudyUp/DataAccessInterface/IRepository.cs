using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DataAccessInterface
{
    public interface IRepository<T>
    {
        void Add(T entity);
        void Delete(T entity);
        bool Exists(T entity);
        ICollection<T> FindByCondition(Expression<Func<T, bool>> condition);
        IEnumerable<T> GetAll();
        T GetById(int id);
        void Save();
        void Update(T entity);
    }
}
