using BusinessLogicInterface;
using DataAccessInterface;
using Domain;
using Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogic
{
    public class UserLogic : IUserLogic
    {
        private IRepository<User> repository;
        private IUserRepository userRepository;

        public UserLogic(IRepository<User> repository, IUserRepository userRepository)
        {
            this.repository = repository;
            this.userRepository = userRepository;
        }

        public User AddUser(User user)
        {
            IEnumerable<User> sameEmail = repository.GetAll().Where(x => x.Email.Equals(user.Email));
            if (sameEmail != null && (sameEmail.Count() > 0))
                throw new AlreadyExistsException(UserMessage.EMAIL_ALREADY_EXISTS);

            user.Token = Guid.NewGuid().ToString();
            repository.Add(user);
            return user;
        }

        public User Login(string email, string password)
        {
            User user = userRepository.GetUserByEmailAndPassword(email, password);
            if (user == null) throw new InvalidException("Wrong email or password");
            user.Token = Guid.NewGuid().ToString();
            repository.Update(user);
            return user;
        }
    }
}
