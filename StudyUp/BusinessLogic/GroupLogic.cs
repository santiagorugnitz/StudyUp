using DataAccessInterface;
using Domain;
using Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLogic
{
    public class GroupLogic
    {
        private IRepository<Group> groupRepository;
        private IRepository<User> userRepository;
        private IUserRepository userTokenRepository;

        public GroupLogic(IRepository<Group> repository, IUserRepository userTokenRepository, IRepository<User> userRepository)
        {
            this.groupRepository = repository;
            this.userRepository = userRepository;
            this.userTokenRepository = userTokenRepository;
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
    }
}
