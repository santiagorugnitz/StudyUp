﻿using BusinessLogicInterface;
using DataAccessInterface;
using Domain;
using Exceptions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace BusinessLogic
{
    public class GroupLogic : IGroupLogic
    {
        private IRepository<Group> groupRepository;
        private IRepository<User> userRepository;
        private IRepository<Deck> deckRepository;
        private IRepository<UserGroup> userGroupRepository;
        private IRepository<DeckGroup> deckGroupRepository;
        private IUserRepository userTokenRepository;
        private INotifications notificationsInterface;

        public GroupLogic(IRepository<Group> repository, IUserRepository userTokenRepository,
            IRepository<User> userRepository, IRepository<UserGroup> userGroupRepository,
           IRepository<Deck> deckRepository, IRepository<DeckGroup> deckGroupRepository,
           INotifications notificationsInterface)
        {
            this.groupRepository = repository;
            this.userRepository = userRepository;
            this.deckRepository = deckRepository;
            this.userTokenRepository = userTokenRepository;
            this.userGroupRepository = userGroupRepository;
            this.deckGroupRepository = deckGroupRepository;
            this.notificationsInterface = notificationsInterface;
        }

        public Group AddGroup(Group group, string creatorsToken)
        {
            User user = userTokenRepository.GetUserByToken(creatorsToken);
            if (user.Groups.Exists(auxGroup => auxGroup.Name == group.Name))
            {
                throw new AlreadyExistsException(GroupMessage.GROUP_ALREADY_EXISTS);
            }

            if (group.Name is null || group.Name.Trim() == "")
            {
                throw new InvalidException(GroupMessage.EMPTY_NAME_MESSAGE);
            }

            group.Creator = user;
            groupRepository.Add(group);

            user.Groups.Add(group);
            userRepository.Update(user);
            return group;
        }

        public IEnumerable<Group> GetAllGroups(string keyword)
        {
            if (keyword == null) keyword = "";
            return groupRepository.FindByCondition(g => g.Name.ToUpper().Contains(keyword.ToUpper()));
        }

        public bool Subscribe(string token, int id)
        {
            User user = userTokenRepository.GetUserByToken(token);
            Group group = groupRepository.GetById(id);

            if (user is null)
                throw new InvalidException(UnauthenticatedMessage.UNAUTHENTICATED_USER);

            if (group is null)
                throw new NotFoundException(GroupMessage.GROUP_NOT_FOUND);

            var resultFind = userGroupRepository.FindByCondition(t => t.GroupId == id
                     && t.UserId == user.Id);

            if (resultFind.Count > 0)
                throw new AlreadyExistsException(GroupMessage.ALREADY_SUBSCRIBED);

            UserGroup userGroup = new UserGroup()
            {
                GroupId = id,
                Group = group,
                User = user,
                UserId = user.Id
            };

            group.UserGroups.Add(userGroup);
            groupRepository.Update(group);
            return true;
        }

        public bool Unsubscribe(string token, int id)
        {
            User user = userTokenRepository.GetUserByToken(token);
            Group group = groupRepository.GetById(id);

            if (user is null)
                throw new InvalidException(UnauthenticatedMessage.UNAUTHENTICATED_USER);

            if (group is null)
                throw new NotFoundException(GroupMessage.GROUP_NOT_FOUND);

            var resultFind = userGroupRepository.FindByCondition(t => t.GroupId == id
                     && t.UserId == user.Id);

            if (resultFind.Count == 0)
                throw new InvalidException(GroupMessage.NOT_SUBSCRIBED);

            userGroupRepository.Delete(resultFind.First());
            group.UserGroups.Remove(resultFind.First());
            groupRepository.Update(group);
            return true;
        }

        public bool UserIsSubscribed(string token, int id)
        {
            User user = userTokenRepository.GetUserByToken(token);

            if (user is null)
                throw new InvalidException(UnauthenticatedMessage.UNAUTHENTICATED_USER);

            var resultFind = userGroupRepository.FindByCondition(t => t.GroupId == id
                                 && t.UserId == user.Id);

            if (resultFind.Count == 0)
                return false;
            else
                return true;
        }

        public Group Assign(string token, int groupId, int deckId)
        {
            User user = userTokenRepository.GetUserByToken(token);
            Deck deck = deckRepository.GetById(deckId);
            Group group = groupRepository.GetById(groupId);

            if (user is null)
                throw new InvalidException(UnauthenticatedMessage.UNAUTHENTICATED_USER);

            if (group is null)
                throw new NotFoundException(GroupMessage.GROUP_NOT_FOUND);

            if (deck is null)
                throw new NotFoundException(DeckMessage.DECK_NOT_FOUND);

            if (!group.Creator.Equals(user))
                throw new InvalidException(GroupMessage.NOT_AUTHORIZED);

            var resultFind = deckGroupRepository.FindByCondition(t => t.GroupId == groupId
                    && t.DeckId == deckId);

            if (resultFind.Count > 0)
                throw new AlreadyExistsException(GroupMessage.ALREADY_ASSIGNED);

            DeckGroup deckGroup = new DeckGroup()
            {
                Deck = deck,
                DeckId = deckId,
                Group = group,
                GroupId = groupId
            };

            this.notificationsInterface.NotifyMaterial(deckId, group);

            group.DeckGroups.Add(deckGroup);
            groupRepository.Update(group);
            return group;
        }

        public Group Unassign(string token, int groupId, int deckId)
        {
            User user = userTokenRepository.GetUserByToken(token);
            Group group = groupRepository.GetById(groupId);

            if (user is null)
                throw new InvalidException(UnauthenticatedMessage.UNAUTHENTICATED_USER);

            if (group is null)
                throw new NotFoundException(GroupMessage.GROUP_NOT_FOUND);

            if (!group.Creator.Equals(user))
                throw new InvalidException(GroupMessage.NOT_AUTHORIZED);

            var resultFind = deckGroupRepository.FindByCondition(t => t.GroupId == groupId
                    && t.DeckId == deckId);

            if (resultFind.Count == 0)
                throw new NotFoundException(GroupMessage.NOT_ASSIGNED);

            deckGroupRepository.Delete(resultFind.First());
            group.DeckGroups.Remove(resultFind.First());
            groupRepository.Update(group);
            return group;
        }

        public IEnumerable<Group> GetTeachersGroups(string token)
        {
            User user = userTokenRepository.GetUserByToken(token);
            return groupRepository.FindByCondition(g => g.Creator.Equals(user));
        }

        public List<Deck> GetGroupDecks(int groupId)
        {
            Group group = groupRepository.GetById(groupId);

            if (group is null)
                throw new NotFoundException(GroupMessage.GROUP_NOT_FOUND);

            var resultFind = deckGroupRepository.FindByCondition(a => a.GroupId == groupId);

            List<Deck> toReturn = new List<Deck>();

            foreach (var deckGroup in resultFind)
            {
                toReturn.Add(this.deckRepository.GetById(deckGroup.DeckId));
            }

            return toReturn;
        }
    }
}