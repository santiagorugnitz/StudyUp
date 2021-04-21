using Domain;
using System;
using System.Collections.Generic;

namespace BusinessLogicInterface
{
    public interface IUserLogic
    {
        User AddUser(User user);
        User Login(string email, string password);
        User LoginByUsername(string username, string password);
        IEnumerable<User> GetUsers(string queryFilter);
        User CheckToken(string token);
        User CheckUsername(string username);
        User FollowUser(string token, string username);
        User UnfollowUser(string token, string username);
    }
}
