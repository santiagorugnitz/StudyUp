using Domain;
using System;
using System.Collections.Generic;

namespace BusinessLogicInterface
{
    public interface IUserLogic
    {
        User AddUser(User user);
        User Login(string email, string password, string firebaseToken);
        User LoginByUsername(string username, string password, string firebaseToken);
        IEnumerable<Deck> GetDecksFromFollowing(string token);
        IEnumerable<Tuple<User, bool>> GetUsers(string token, string queryFilter);
        User CheckToken(string token);
        User CheckUsername(string username);
        User FollowUser(string token, string username);
        User UnfollowUser(string token, string username);
    }
}
