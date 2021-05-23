using Domain;
using System.Collections.Generic;

namespace BusinessLogicInterface
{
    public interface IGroupLogic
    {
        Group AddGroup(Group group, string creatorsToken);
        IEnumerable<Group> GetAllGroups(string keyword);
        List<Deck> GetGroupDecks(int groupId);
        IEnumerable<Group> GetTeachersGroups(string keyword);
        bool Subscribe(string token, int id);
        bool Unsubscribe(string token, int id);
        bool UserIsSubscribed(string token, int id);
    }
}
