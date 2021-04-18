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
        private ValidationService validationService;

        public UserLogic(IRepository<User> repository, IUserRepository userRepository)
        {
            this.repository = repository;
            this.userRepository = userRepository;
            validationService = new ValidationService();
        }

        public User AddUser(User user)
        {
            IEnumerable<User> sameEmail = repository.GetAll().Where(x => x.Email.Equals(user.Email));
            if (sameEmail != null && (sameEmail.Count() > 0))
                throw new AlreadyExistsException(UserMessage.EMAIL_ALREADY_EXISTS);

            if (!validationService.PasswordValidation(user.Password))
                throw new InvalidException(UserMessage.INVALID_PASSWORD);

            if (!validationService.EmailValidation(user.Email))
                throw new InvalidException(UserMessage.INVALID_EMAIL);

            user.Token = Guid.NewGuid().ToString();
            repository.Add(user);
            return user;

        }

        public User Login(string email, string password)
        {
            User user = userRepository.GetUserByEmailAndPassword(email, password);
            if (user == null) throw new InvalidException(UserMessage.WRONG_EMAIL_OR_PASSWORD);
            return GenerateToken(user);
        }

        public User LoginByUsername(string username, string password)
        {
            User user = userRepository.GetUserByNameAndPassword(username, password);
            if (user == null) throw new InvalidException(UserMessage.WRONG_USERNAME_OR_PASSWORD);
            return GenerateToken(user);
        }

        private User GenerateToken(User user)
        {
            user.Token = Guid.NewGuid().ToString();
            repository.Update(user);
            return user;
        }
    }
}
