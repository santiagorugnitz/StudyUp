using Domain;
using System.Collections.Generic;

namespace BusinessLogicInterface
{
    public interface IGroupLogic
    {
        Group AddGroup(Group group, string creatorsToken);
        bool Subscribe(string token, int id);
        bool Unsubscribe(string token, int id);
        bool UserIsSubscribed(string token, int id);
        IEnumerable<Group> GetAllGroups(string keyword);
        IEnumerable<Group> GetTeachersGroups(string keyword);
        List<Deck> GetGroupDecks(int groupId);
    }
}
