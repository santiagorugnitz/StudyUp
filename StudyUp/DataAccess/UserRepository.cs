using DataAccessInterface;
using Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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

        public User GetUserByNameAndPassword(string username, string password)
        {
            return dbSet.Where(x => x.Username == username && x.Password == password).FirstOrDefault();
        }

        public User GetUserByToken(string token)
        {
            if (token == null)
            {
                return null;
            }

            return dbSet.Where(x => x.Token == token).FirstOrDefault();
        }
    }
}
