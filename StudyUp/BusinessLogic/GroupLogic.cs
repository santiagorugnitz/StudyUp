using BusinessLogicInterface;
using DataAccessInterface;
using Domain;
using Exceptions;
using System.Collections.Generic;
using System.Linq;

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