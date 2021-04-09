using Microsoft.EntityFrameworkCore;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DataAccessInterface;

namespace DataAccess
{
    public class Repository<T> : DataAccessInterface.IRepository<T> where T : class
    {
        private readonly DbSet<T> dbSet;
        private readonly DbContext context;

        public Repository()
        {
        }

        public Repository(DbContext context)
        {
            this.dbSet = context.Set<T>();
            this.context = context;
        }

        public void Add(T entity)
        {
            dbSet.Add(entity);
            Save();
        }

        public void Delete(T entity)
        {
            dbSet.Remove(entity);
            Save();
        }

        public bool Exists(T entity)
        {
            if (dbSet.Find(entity) != null)
            {
                return true;
            }
            return false;
        }

        public ICollection<T> FindByCondition(Expression<Func<T, bool>> condition)
        {
            return context.Set<T>().Where(condition).ToList<T>();
        }

        public IEnumerable<T> GetAll()
        {
            return dbSet.ToList();
        }

        public T GetById(int id)
        {
            return dbSet.Find(id);
        }

        public void Save()
        {
            try
            {
                context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void Update(T entity)
        {
            dbSet.Update(entity);
            Save();
        }
    }
}