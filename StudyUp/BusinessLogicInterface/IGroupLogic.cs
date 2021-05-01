﻿using Domain;
using System;
using System.Collections.Generic;
using System.Text;

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
        Group Assign(string token, int groupId, int deckId);
        Group Unassign(string token, int groupId, int deckId);
        List<Deck> GetGroupDecks(int groupId);
    }
}