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
        private IRepository<UserFollowing> userFollowingRepository;
        private IRepository<UserExam> userExamRepository;
        private ValidationService validationService;

        public UserLogic(IRepository<User> repository, IUserRepository userRepository,
            IRepository<UserExam> userExamRepository, IRepository<UserFollowing> userFollowingRepository)
        {
            this.repository = repository;
            this.userRepository = userRepository;
            this.userExamRepository = userExamRepository;
            this.userFollowingRepository = userFollowingRepository;
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

            UserFollowing follow = new UserFollowing
            {
                FollowingUserId = userToFollow.Id,
                FollowingUser = userToFollow,
                FollowerUserId = authenticatedUser.Id,
                FollowerUser = authenticatedUser
            };

            if (authenticatedUser.FollowedUsers.Contains(follow))
            {
                throw new InvalidException(UserMessage.ALREADY_FOLLOWS);
            }

            userToFollow.FollowingUsers.Add(follow);
            repository.Update(userToFollow);
            authenticatedUser.FollowedUsers.Add(follow);

            repository.Update(authenticatedUser);
            return authenticatedUser;
        }

        public User UnfollowUser(string token, string username)
        {
            User authenticatedUser = CheckToken(token);
            User userToUnfollow = CheckUsername(username);

            UserFollowing unfollow = new UserFollowing
            {
                FollowingUserId = userToUnfollow.Id,
                FollowingUser = userToUnfollow,
                FollowerUserId = authenticatedUser.Id,
                FollowerUser = authenticatedUser
            };

            if (!authenticatedUser.FollowedUsers.Contains(unfollow))
            {
                throw new InvalidException(UserMessage.NOT_FOLLOWS);
            }

            userToUnfollow.FollowingUsers.Remove(unfollow);
            repository.Update(userToUnfollow);
            authenticatedUser.FollowedUsers.Remove(unfollow);

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

        public IEnumerable<Tuple<User, bool>> GetUsers(string token, string queryFilter)
        {
            User authenticatedUser = CheckToken(token);

            if (queryFilter.Length == 0)
            {
                var list = repository.FindByCondition(user => user.Id != authenticatedUser.Id);
                var orderedList = list.OrderBy(user => user.Username.Length);
                return GetListWithFollowingFilter(authenticatedUser, orderedList);
            }

            var filteredList = repository.FindByCondition(user => user.Username.Contains(queryFilter) && user.Id != authenticatedUser.Id);
            var orderedFilteredList = filteredList.OrderBy(user => user.Username.Length);

            return GetListWithFollowingFilter(authenticatedUser, orderedFilteredList);
        }

        private IEnumerable<Tuple<User, bool>> GetListWithFollowingFilter(User user, IEnumerable<User> convertingList)
        {
            List<Tuple<User, bool>> convertedList = new List<Tuple<User, bool>>();

            foreach (var listedUser in convertingList)
            {
                if (user.FollowedUsers.FindAll(user => user.FollowingUserId == listedUser.Id).Count() > 0)
                {
                    convertedList.Add(new Tuple<User, bool>(listedUser, true));
                }
                else
                {
                    convertedList.Add(new Tuple<User, bool>(listedUser, false));
                }
            }

            return convertedList;
        }

        public User Login(string email, string password, string firebaseToken)
        {
            User user = userRepository.GetUserByEmailAndPassword(email, password);
            if (user == null) throw new InvalidException(UserMessage.WRONG_EMAIL_OR_PASSWORD);
            UpdateFirebaseToken(user, firebaseToken);
            return GenerateToken(user);
        }

        public User LoginByUsername(string username, string password, string firebaseToken)
        {
            User user = userRepository.GetUserByNameAndPassword(username, password);
            if (user == null) throw new InvalidException(UserMessage.WRONG_USERNAME_OR_PASSWORD);
            UpdateFirebaseToken(user, firebaseToken);
            return GenerateToken(user);
        }

        private User GenerateToken(User user)
        {
            user.Token = Guid.NewGuid().ToString();
            repository.Update(user);
            return user;
        }

        private void UpdateFirebaseToken(User user, string firebaseToken)
        {
            user.FirebaseToken = firebaseToken;
            repository.Update(user);
        }

        public IEnumerable<Deck> GetDecksFromFollowing(string token)
        {
            User authenticatedUser = CheckToken(token);
            List<Deck> deckFromFollowing = new List<Deck>();

            foreach (var user in authenticatedUser.FollowedUsers)
            {
                foreach (var deck in user.FollowingUser.Decks)
                {
                    if (!deck.IsHidden) deckFromFollowing.Add(deck);
                }
            }

            return deckFromFollowing;
        }

        public Tuple<List<Deck>, List<Exam>> GetTasks(string token)
        {
            User authenticatedUser = CheckToken(token);
            List<Deck> decks = new List<Deck>();
            List<Exam> exams = new List<Exam>();

            foreach (var iter in authenticatedUser.UserGroups)
            {
                Group group = iter.Group;
                foreach (DeckGroup deckGroup in group.DeckGroups)
                {
                    decks.Add(deckGroup.Deck);
                }

                foreach (Exam exam in group.AssignedExams)
                {
                    if (!MadeExam(authenticatedUser, exam))
                    {
                        exams.Add(exam);
                    }
                }
            }

            return new Tuple<List<Deck>, List<Exam>>(decks, exams);
        }

        private bool MadeExam(User user, Exam exam)
        {
            UserExam obtainedExam;
            try
            {
                obtainedExam = user.SolvedExams.Find(solved => solved.ExamId == exam.Id && solved.UserId == user.Id);
            }
            catch (NullReferenceException)
            {
                return false;
            }

            if (obtainedExam != null && obtainedExam.Score != null)
            {
                return true;
            }

            return false;
        }

        public List<User> GetUsersForRanking(string token)
        {
            User authenticatedUser = CheckToken(token);
            ICollection<UserFollowing> following =
                userFollowingRepository.FindByCondition(a => a.FollowerUserId == authenticatedUser.Id);

            List<User> toReturn = new List<User>();

            if (!(following is null))
            {
                foreach (UserFollowing userFollowing in following)
                {
                    User user = repository.GetById(userFollowing.FollowingUserId);
                    if (user.IsStudent)
                        toReturn.Add(user);
                }
            }
            toReturn.Add(authenticatedUser);

            return toReturn;
        }

        public double GetScore(string username)
        {
            User user = repository.FindByCondition(a => a.Username.Equals(username)).FirstOrDefault();

            if (user is null)
                throw new InvalidException(UserMessage.USER_NOT_FOUND);

            ICollection<UserExam> usersExams = userExamRepository.FindByCondition(f => f.UserId == user.Id);

            double score = 0;
            foreach (UserExam userExam in usersExams)
                score += (int)userExam.Score;

            return score;
        }
    }
}
