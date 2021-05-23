using Domain;
using System;
using System.Collections.Generic;

namespace BusinessLogicInterface
{
    public interface IUserLogic
    {
        User AddUser(User user);
        User CheckToken(string token);
        User CheckUsername(string username);
        User FollowUser(string token, string username);
        IEnumerable<Deck> GetDecksFromFollowing(string token);
        Double GetScore(string username);
        Tuple<List<Deck>, List<Exam>> GetTasks(string token);
        IEnumerable<Tuple<User, bool>> GetUsers(string token, string queryFilter);
        List<User> GetUsersForRanking(string token);
        User Login(string email, string password, string firebaseToken);
        User LoginByUsername(string username, string password, string firebaseToken);
        void Logout(string token);
        User UnfollowUser(string token, string username);
    }
}
