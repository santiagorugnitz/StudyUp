using Domain;
using System;

namespace DataAccessInterface
{
    public interface IUserRepository
    {
        public User GetUserByEmailAndPassword(string email, string password);
        public User GetUserByNameAndPassword(string username, string password);
        public User GetUserByToken(string token);
    }
}
