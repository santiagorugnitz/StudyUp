using BusinessLogic;
using DataAccessInterface;
using Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Exceptions;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace BusinessLogicTest
{
    [TestClass]
    public class UserLogicTest
    {
        User userExample;
        User userListed;
        IEnumerable<User> userList;
        Mock<IUserRepository> userMock;
        Mock<IRepository<User>> userRepositoryMock;
        Mock<IRepository<UserFollowing>> userFollowingRepositoryMock;
        Mock<IRepository<UserExam>> userExamRepositoryMock;
        UserLogic userLogic;

        [TestInitialize]
        public void SetUp()
        {
            userListed = new User()
            {
                Username = "Maria Fernanda",
                Email = "mariaFer@gmail.com",
                Password = "Mari1k",
                IsStudent = false,
                Token = "New token",
                FollowingUsers = new List<UserFollowing>(),
                FollowedUsers = new List<UserFollowing>()
            };

            userExample = new User()
            {
                Id = 1,
                Username = "Maria",
                Email = "maria@gmail.com",
                Password = "Mari1k",
                IsStudent = false,
                Token = "New token",
                FollowingUsers = new List<UserFollowing>(),
                FollowedUsers = new List<UserFollowing>()
            };

            UserFollowing userFollowingListed = new UserFollowing
            {
                FollowingUserId = userListed.Id,
                FollowingUser = userListed,
                FollowerUserId = userExample.Id,
                FollowerUser = userExample
            };

            userExample.FollowedUsers = new List<UserFollowing>() { userFollowingListed };

            userList = new List<User>() { userListed, userExample };
            userMock = new Mock<IUserRepository>(MockBehavior.Strict);
            userRepositoryMock = new Mock<IRepository<User>>(MockBehavior.Loose);
            userExamRepositoryMock = new Mock<IRepository<UserExam>>(MockBehavior.Loose);
            userFollowingRepositoryMock = new Mock<IRepository<UserFollowing>>(MockBehavior.Loose);
            userLogic = new UserLogic(userRepositoryMock.Object, userMock.Object, 
                userExamRepositoryMock.Object, userFollowingRepositoryMock.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(AlreadyExistsException))]
        public void AddUserTestRepeatedEmail()
        {
            userRepositoryMock.Setup(m => m.Add(It.IsAny<User>()));
            userRepositoryMock.Setup(m => m.GetAll()).Returns(new List<User>() { userExample });

            var result = userLogic.AddUser(userExample);

            userRepositoryMock.VerifyAll();
            Assert.AreEqual(userExample, result);
        }

        [TestMethod]
        [ExpectedException(typeof(AlreadyExistsException))]
        public void AddUserTestRepeatedUsername()
        {
            userRepositoryMock.Setup(m => m.Add(It.IsAny<User>()));
            userRepositoryMock.Setup(m => m.GetAll()).Returns(new List<User>() { userExample });

            var result = userLogic.AddUser(userExample);

            userRepositoryMock.VerifyAll();
            Assert.AreEqual(userExample, result);
        }

        [TestMethod]
        public void AddUserTest()
        {
            userRepositoryMock.Setup(m => m.Add(It.IsAny<User>()));
            userRepositoryMock.Setup(m => m.GetAll()).Returns(new List<User>());

            var result = userLogic.AddUser(userExample);

            userRepositoryMock.VerifyAll();
            Assert.AreEqual(userExample, result);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidException))]
        public void AddUserInvalidPasswordTest()
        {
            User userToAdd = new User()
            {
                Email = "ana@gmail.com",
                IsStudent = false,
                Password = "Incorrect",
                Username = "anaanaana",
                Token = "new token"
            };

            userRepositoryMock.Setup(m => m.Add(It.IsAny<User>()));
            userRepositoryMock.Setup(m => m.GetAll()).Returns(new List<User>() { userExample });

            var result = userLogic.AddUser(userToAdd);
            userRepositoryMock.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidException))]
        public void AddUserInvalidEmailTest()
        {
            User invalidEmailUser = new User()
            {
                Email = "jose.com",
                IsStudent = false,
                Password = "Password1-",
                Username = "Jose",
                Token = "new token"
            };

            userRepositoryMock.Setup(m => m.Add(It.IsAny<User>()));
            userRepositoryMock.Setup(m => m.GetAll()).Returns(new List<User>() { userExample });

            var result = userLogic.AddUser(invalidEmailUser);
            userRepositoryMock.VerifyAll();
        }

        [TestMethod]
        public void CorrectLoginTest()
        {
            userMock.Setup(m => m.GetUserByEmailAndPassword(It.IsAny<string>(), It.IsAny<string>())).Returns(userExample);

            var result = userLogic.Login("mail", "password", "token");

            userMock.VerifyAll();
            Assert.AreEqual(userExample, result);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidException))]
        public void InCorrectMailOrPasswordLoginTest()
        {
            userMock.Setup(m => m.GetUserByEmailAndPassword(It.IsAny<string>(), It.IsAny<string>())).Returns((User)null);

            var result = userLogic.Login("mail", "password", "token");

            userMock.VerifyAll();
        }

        [TestMethod]
        public void CorrectLoginByUsernameTest()
        {
            userMock.Setup(m => m.GetUserByNameAndPassword(It.IsAny<string>(), It.IsAny<string>())).Returns(userExample);

            var result = userLogic.LoginByUsername("username", "password", "token");

            userMock.VerifyAll();
            Assert.AreEqual(userExample, result);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidException))]
        public void InCorrectUsernameOrPasswordLoginTest()
        {
            userMock.Setup(m => m.GetUserByNameAndPassword(It.IsAny<string>(), It.IsAny<string>())).Returns((User)null);

            var result = userLogic.LoginByUsername("username", "password", "token");

            userMock.VerifyAll();
        }

        [TestMethod]
        public void GetUsersByName()
        {
            userMock.Setup(m => m.GetUserByToken(It.IsAny<string>())).Returns(userExample);
            userRepositoryMock.Setup(m => m.FindByCondition(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns((ICollection<User>)userList);

            var result = userLogic.GetUsers("Token", "Maria");

            userMock.VerifyAll();
            userRepositoryMock.VerifyAll();

            Assert.AreEqual(2, result.Count());
            Assert.AreEqual("Maria", result.ElementAt(0).Item1.Username.ToString());
            Assert.AreEqual("Maria Fernanda", result.ElementAt(1).Item1.Username.ToString());
            Assert.AreEqual(false, result.ElementAt(0).Item2);
            Assert.AreEqual(true, result.ElementAt(1).Item2);
        }

        [TestMethod]
        public void GetAllUsers()
        {
            userMock.Setup(m => m.GetUserByToken(It.IsAny<string>())).Returns(userExample);
            userRepositoryMock.Setup(m => m.FindByCondition(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns((ICollection<User>)userList);

            var result = userLogic.GetUsers("Token", "");

            userMock.VerifyAll();
            userRepositoryMock.VerifyAll();

            Assert.AreEqual(2, result.Count());
            Assert.AreEqual("Maria", result.ElementAt(0).Item1.Username.ToString());
            Assert.AreEqual("Maria Fernanda", result.ElementAt(1).Item1.Username.ToString());
            Assert.AreEqual(false, result.ElementAt(0).Item2);
            Assert.AreEqual(true, result.ElementAt(1).Item2);
        }

        [TestMethod]
        public void CheckUsernameOk()
        {
            userRepositoryMock.Setup(m => m.FindByCondition(user => user.Username.Equals("Maria"))).Returns(new List<User>() { userExample });

            var result = userLogic.CheckUsername("Maria");

            userRepositoryMock.VerifyAll();

            Assert.AreEqual(userExample, result);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidException))]
        public void CheckUsernameInvalid()
        {
            userRepositoryMock.Setup(m => m.FindByCondition(user => user.Username.Equals("Maria"))).Returns(new List<User>() { });

            var result = userLogic.CheckUsername("Maria");

            userRepositoryMock.VerifyAll();
        }

        [TestMethod]
        public void CheckTokenOk()
        {
            userMock.Setup(m => m.GetUserByToken(It.IsAny<string>())).Returns(userExample);

            var result = userLogic.CheckToken("Token");

            userRepositoryMock.VerifyAll();

            Assert.AreEqual(userExample, result);
        }

        [TestMethod]
        [ExpectedException(typeof(NotAuthenticatedException))]
        public void CheckTokenInvalid()
        {
            userMock.Setup(m => m.GetUserByToken(It.IsAny<string>())).Returns((User)null);

            var result = userLogic.CheckToken("Token");

            userRepositoryMock.VerifyAll();
        }

        [TestMethod]
        public void FollowUserOk()
        {
            User userToFollow = new User()
            {
                Id = 1,
                Username = "Ana",
                Email = "ana@gmail.com",
                Password = "Anaaaa123",
                IsStudent = false,
                Token = "New token",
                FollowingUsers = new List<UserFollowing>(),
                FollowedUsers = new List<UserFollowing>()
            };

            userExample.FollowedUsers = new List<UserFollowing>();
            userRepositoryMock.Setup(m => m.FindByCondition(user => user.Username.Equals("Ana"))).Returns(new List<User>() { userToFollow });
            userMock.Setup(m => m.GetUserByToken(It.IsAny<string>())).Returns(userExample);

            var result = userLogic.FollowUser(userExample.Token, userToFollow.Username);

            userRepositoryMock.VerifyAll();

            Assert.AreEqual(userExample, result);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidException))]
        public void FollowUserInvalid()
        {
            UserFollowing userFollowingExample = new UserFollowing
            {
                FollowingUserId = userExample.Id,
                FollowingUser = userExample,
                FollowerUserId = userExample.Id,
                FollowerUser = userExample
            };

            userExample.FollowedUsers = new List<UserFollowing>() { userFollowingExample };

            userRepositoryMock.Setup(m => m.FindByCondition(user => user.Username.Equals("Maria"))).Returns(new List<User>() { userExample });
            userMock.Setup(m => m.GetUserByToken(It.IsAny<string>())).Returns(userExample);

            var result = userLogic.FollowUser("Token", "Maria");

            userRepositoryMock.VerifyAll();
        }

        [TestMethod]
        public void UnfollowUserOk()
        {
            UserFollowing userFollowingExample = new UserFollowing
            {
                FollowingUserId = userExample.Id,
                FollowingUser = userExample,
                FollowerUserId = userExample.Id,
                FollowerUser = userExample
            };

            userExample.FollowedUsers = new List<UserFollowing>() { userFollowingExample };
            userRepositoryMock.Setup(m => m.FindByCondition(user => user.Username.Equals("Maria"))).Returns(new List<User>() { userExample });
            userMock.Setup(m => m.GetUserByToken(It.IsAny<string>())).Returns(userExample);

            var result = userLogic.UnfollowUser("Token", "Maria");

            userRepositoryMock.VerifyAll();

            Assert.AreEqual(userExample, result);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidException))]
        public void UnfollowUserInvalid()
        {
            userExample.FollowedUsers = new List<UserFollowing>() { };
            userRepositoryMock.Setup(m => m.FindByCondition(user => user.Username.Equals("Maria"))).Returns(new List<User>() { userExample });
            userMock.Setup(m => m.GetUserByToken(It.IsAny<string>())).Returns(userExample);

            var result = userLogic.UnfollowUser("Token", "Maria");

            userRepositoryMock.VerifyAll();
        }

        [TestMethod]
        public void GetDecksFromFollowing()
        {
            userListed.Decks = new List<Deck>() { new Deck() { Name = "Deck1", IsHidden = true }, new Deck() { Name = "Deck2", IsHidden = false } };
            UserFollowing userFollowingListed = new UserFollowing
            {
                FollowingUserId = userListed.Id,
                FollowingUser = userListed,
                FollowerUserId = userExample.Id,
                FollowerUser = userExample
            };

            userExample.FollowedUsers = new List<UserFollowing>() { userFollowingListed };
            userMock.Setup(m => m.GetUserByToken(It.IsAny<string>())).Returns(userExample);

            var result = userLogic.GetDecksFromFollowing("Token");

            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("Deck2", result.ElementAt(0).Name);
            userMock.VerifyAll();
        }

        [TestMethod]
        public void GetTasksDeck()
        {
            var deckExample = new Deck()
            {
                Id = 1,
                Name = "Clase 7",
                Author = userExample,
                Difficulty = Domain.Enumerations.Difficulty.Medium,
                IsHidden = false,
                Subject = "English",
                Flashcards = new List<Flashcard>()
            };

            var group = new Group()
            {
                Id = 1,
                Name = "Clase 7",
                Creator = userExample,
                UserGroups = new List<UserGroup>(),
                DeckGroups = new List<DeckGroup>(),
                AssignedExams = new List<Exam>()
            };

            var deckGroups = new List<DeckGroup>() { new DeckGroup() { Group = group, Deck = deckExample, DeckId = deckExample.Id, GroupId = group.Id } };
            group.DeckGroups = deckGroups;
            userExample.Groups = new List<Group>() { group };
            userExample.UserGroups = new List<UserGroup>() { new UserGroup() { User = userExample, UserId = userExample.Id, Group = group, GroupId = group.Id } };

            userMock.Setup(m => m.GetUserByToken(It.IsAny<string>())).Returns(userExample);

            var result = userLogic.GetTasks("token");

            Assert.AreEqual(1, result.Item1.Count());
            Assert.AreEqual(deckExample, result.Item1.ElementAt(0));
            Assert.AreEqual(0, result.Item2.Count());
            userMock.VerifyAll();
        }

        [TestMethod]
        public void GetTasksExam()
        {
            var examExample = new Exam()
            {
                Id = 1,
                Name = "Exam1",
                Author = userExample,
                Difficulty = Domain.Enumerations.Difficulty.Medium,
                Subject = "English",
                ExamCards = new List<ExamCard>()
            };

            var group = new Group()
            {
                Id = 1,
                Name = "Clase 7",
                Creator = userExample,
                UserGroups = new List<UserGroup>(),
                DeckGroups = new List<DeckGroup>(),
                AssignedExams = new List<Exam>() { examExample }
            };

            userExample.Groups = new List<Group>() { group };
            userExample.UserGroups = new List<UserGroup>() { new UserGroup() { User = userExample, UserId = userExample.Id, Group = group, GroupId = group.Id } };

            userMock.Setup(m => m.GetUserByToken(It.IsAny<string>())).Returns(userExample);

            var result = userLogic.GetTasks("token");

            Assert.AreEqual(0, result.Item1.Count());
            Assert.AreEqual(1, result.Item2.Count());
            Assert.AreEqual(examExample, result.Item2.ElementAt(0));

            userMock.VerifyAll();
        }

        [TestMethod]
        public void GetTasksOneExam()
        {
            var examExample = new Exam()
            {
                Id = 1,
                Name = "Exam1",
                Author = userExample,
                Difficulty = Domain.Enumerations.Difficulty.Medium,
                Subject = "English",
                ExamCards = new List<ExamCard>()
            };

            var group = new Group()
            {
                Id = 1,
                Name = "Clase 7",
                Creator = userExample,
                UserGroups = new List<UserGroup>(),
                DeckGroups = new List<DeckGroup>(),
                AssignedExams = new List<Exam>() { examExample }
            };

            userExample.Groups = new List<Group>() { group };
            userExample.SolvedExams = new List<UserExam>() { new UserExam() { Exam = examExample, User = userExample,
                ExamId = examExample.Id, UserId = userExample.Id, Score = null} };
            userExample.UserGroups = new List<UserGroup>() { new UserGroup() { User = userExample, UserId = userExample.Id, Group = group, GroupId = group.Id } };

            userMock.Setup(m => m.GetUserByToken(It.IsAny<string>())).Returns(userExample);

            var result = userLogic.GetTasks("token");

            Assert.AreEqual(0, result.Item1.Count());
            Assert.AreEqual(1, result.Item2.Count());
            Assert.AreEqual(examExample, result.Item2.ElementAt(0));

            userMock.VerifyAll();
        }

        [TestMethod]
        public void GetTasksNotMadeExam()
        {
            var examExample = new Exam()
            {
                Id = 1,
                Name = "Exam1",
                Author = userExample,
                Difficulty = Domain.Enumerations.Difficulty.Medium,
                Subject = "English",
                ExamCards = new List<ExamCard>()
            };

            var group = new Group()
            {
                Id = 1,
                Name = "Clase 7",
                Creator = userExample,
                UserGroups = new List<UserGroup>(),
                DeckGroups = new List<DeckGroup>(),
                AssignedExams = new List<Exam>() { examExample }
            };

            userExample.Groups = new List<Group>() { group };
            userExample.SolvedExams = new List<UserExam>() { new UserExam() { Exam = examExample, User = userExample,
                ExamId = examExample.Id, UserId = userExample.Id, Score = null} };
            userExample.UserGroups = new List<UserGroup>() { new UserGroup() { User = userExample, UserId = userExample.Id, Group = group, GroupId = group.Id } };

            userMock.Setup(m => m.GetUserByToken(It.IsAny<string>())).Returns(userExample);

            var result = userLogic.GetTasks("token");

            Assert.AreEqual(0, result.Item1.Count());
            Assert.AreEqual(1, result.Item2.Count());
            Assert.AreEqual(examExample, result.Item2.ElementAt(0));

            userMock.VerifyAll();
        }
        
        [TestMethod]
        public void GetTasksAlreadyMadeExam()
        {
            var examExample = new Exam()
            {
                Id = 1,
                Name = "Exam1",
                Author = userExample,
                Difficulty = Domain.Enumerations.Difficulty.Medium,
                Subject = "English",
                ExamCards = new List<ExamCard>()
            };

            var group = new Group()
            {
                Id = 1,
                Name = "Clase 7",
                Creator = userExample,
                UserGroups = new List<UserGroup>(),
                DeckGroups = new List<DeckGroup>(),
                AssignedExams = new List<Exam>() { examExample }
            };

            userExample.Groups = new List<Group>() { group };
            userExample.SolvedExams = new List<UserExam>() { new UserExam() { Exam = examExample, User = userExample,
                ExamId = examExample.Id, UserId = userExample.Id, Score = 5} };
            userExample.UserGroups = new List<UserGroup>() { new UserGroup() { User = userExample, UserId = userExample.Id, Group = group, GroupId = group.Id } };

            userMock.Setup(m => m.GetUserByToken(It.IsAny<string>())).Returns(userExample);

            var result = userLogic.GetTasks("token");

            Assert.AreEqual(0, result.Item1.Count());
            Assert.AreEqual(0, result.Item2.Count());
            
            userMock.VerifyAll();
        }

        [TestMethod]
        public void GetScoreTest()
        {
            userRepositoryMock.Setup(a => a.FindByCondition(It.IsAny<Expression<Func<User,
                bool>>>())).Returns(new List<User>() { userExample});
            userExamRepositoryMock.Setup(a => a.FindByCondition(It.IsAny<Expression<Func<UserExam,
               bool>>>())).Returns(new List<UserExam>() { new UserExam() { Score = 10 } });

            var result = userLogic.GetScore(userExample.Username);

            Assert.AreEqual(10, result);
            userRepositoryMock.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidException))]
        public void GetScoreNullUserTest()
        {
            userRepositoryMock.Setup(a => a.FindByCondition(It.IsAny<Expression<Func<User,
                bool>>>())).Returns(new List<User>() {});
            
            var result = userLogic.GetScore(userExample.Username);
        }

        [TestMethod]
        public void GetUsersForRankingTest()
        {
            userFollowingRepositoryMock.Setup(a => a.FindByCondition(It.IsAny<Expression<Func<UserFollowing,
                bool>>>())).Returns(new List<UserFollowing>() { new UserFollowing() { FollowerUserId = 1, FollowingUserId = 2} });
            userMock.Setup(u => u.GetUserByToken(It.IsAny<string>())).Returns(userExample);
            userRepositoryMock.Setup(u => u.GetById(It.IsAny<int>())).Returns(userExample);

            var result = userLogic.GetUsersForRanking("token");

            Assert.AreEqual(1, result.Count());
            userFollowingRepositoryMock.VerifyAll();
        }

        [TestMethod]
        public void MakeLogout()
        {
            userMock.Setup(u => u.GetUserByToken(It.IsAny<string>())).Returns(userExample);

            userLogic.Logout("token");

            Assert.AreEqual(null, userExample.FirebaseToken);
            userMock.VerifyAll();
        }
    }
}
