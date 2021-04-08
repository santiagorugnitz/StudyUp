using DataAccessInterface;
using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly DbContext context;
        private readonly DbSet<User> dbSet;

        public UserRepository(DbContext context)
        {
            this.dbSet = context.Set<User>();
            this.context = context;
        }

        public User GetUserByEmailAndPassword(string email, string password)
        {
            return dbSet.Where(x => x.Email == email && x.Password == password).FirstOrDefault();
        }

        public User GetUserByNameAndPassword(string name, string password)
        {
            return dbSet.Where(x => x.Name == name && x.Password == password).FirstOrDefault();
        }
    }
}
