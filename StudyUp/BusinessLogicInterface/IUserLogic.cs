using Domain;
using System;

namespace BusinessLogicInterface
{
    public interface IUserLogic
    {
        User AddUser(User user);
        User Login(string email, string password);
    }
}
