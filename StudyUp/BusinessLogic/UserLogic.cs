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

        public User FollowUser(string token, string username)
        {
            User authenticatedUser = CheckToken(token);
            User userToFollow = CheckUsername(username);

            if (authenticatedUser.FollowedUsers.Contains(userToFollow))
            {
                throw new InvalidException(UserMessage.ALREADY_FOLLOWS);
            }
            
            authenticatedUser.FollowedUsers.Add(userToFollow);

            repository.Update(authenticatedUser);
            return authenticatedUser;
        }

        public User UnfollowUser(string token, string username)
        {
            User authenticatedUser = CheckToken(token);
            User userToUnfollow = CheckUsername(username);

            if (!authenticatedUser.FollowedUsers.Contains(userToUnfollow))
            {
                throw new InvalidException(UserMessage.NOT_FOLLOWS);
            }

            authenticatedUser.FollowedUsers.Remove(userToUnfollow);

            repository.Update(authenticatedUser);
            return authenticatedUser;
        }

        public User CheckToken(string token)
        {
            User user = userRepository.GetUserByToken(token);
            if (user == null)
            {
                throw new NotAuthenticatedException(UnauthenticatedMessage.UNAUTHENTICATED_USER);
            }
            return user;
        }

        public User CheckUsername(string username)
        {
            IEnumerable<User> usernamedUsers = repository.FindByCondition(user => user.Username.Equals(username));
            if (usernamedUsers.Count() < 1)
            {
                throw new InvalidException(UserMessage.USER_NOT_FOUND);
            }
            return usernamedUsers.First();
        }

        public IEnumerable<User> GetUsers(string queryFilter)
        {
            if(queryFilter.Length == 0)
            {
                return repository.GetAll().OrderBy(user => user.Username.Length);
            } 

            return repository.FindByCondition(user => user.IsStudent && user.Username.Contains(queryFilter))
                .OrderBy(user => user.Username.Length);
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
