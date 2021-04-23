using BusinessLogicInterface;
using DataAccessInterface;
using Domain;
using Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLogic
{
    public class GroupLogic : IGroupLogic
    {
        private IRepository<Group> groupRepository;
        private IRepository<User> userRepository;
        private IRepository<UserGroup> userGroupRepository;
        private IUserRepository userTokenRepository;

        public GroupLogic(IRepository<Group> repository, IUserRepository userTokenRepository,
            IRepository<User> userRepository, IRepository<UserGroup> userGroupRepository)
        {
            this.groupRepository = repository;
            this.userRepository = userRepository;
            this.userTokenRepository = userTokenRepository;
            this.userGroupRepository = userGroupRepository;
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
    }
}
