using BusinessLogic;
using DataAccessInterface;
using Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Exceptions;
using System.Collections.Generic;
using System;
using System.Linq;

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
        UserLogic userLogic;

        [TestInitialize]
        public void SetUp()
        {
            userExample = new User()
            {
                Username = "Maria",
                Email = "maria@gmail.com",
                Password = "Mari1k",
                IsStudent = false,
                Token = "New token"
            };

            userListed = new User()
            {
                Username = "Maria Fernanda",
                Email = "maria@gmail.com",
                Password = "Mari1k",
                IsStudent = false,
                Token = "New token"
            };

            userList = new List<User>() { userListed, userExample };
            userMock = new Mock<IUserRepository>(MockBehavior.Strict);
            userRepositoryMock = new Mock<IRepository<User>>(MockBehavior.Loose);
            userLogic = new UserLogic(userRepositoryMock.Object, userMock.Object);
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

            var result = userLogic.Login("mail", "password");

            userMock.VerifyAll();
            Assert.AreEqual(userExample, result);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidException))]
        public void InCorrectMailOrPasswordLoginTest()
        {
            userMock.Setup(m => m.GetUserByEmailAndPassword(It.IsAny<string>(), It.IsAny<string>())).Returns((User)null);

            var result = userLogic.Login("mail", "password");

            userMock.VerifyAll();
        }

        [TestMethod]
        public void CorrectLoginByUsernameTest()
        {
            userMock.Setup(m => m.GetUserByNameAndPassword(It.IsAny<string>(), It.IsAny<string>())).Returns(userExample);

            var result = userLogic.LoginByUsername("username", "password");

            userMock.VerifyAll();
            Assert.AreEqual(userExample, result);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidException))]
        public void InCorrectUsernameOrPasswordLoginTest()
        {
            userMock.Setup(m => m.GetUserByNameAndPassword(It.IsAny<string>(), It.IsAny<string>())).Returns((User)null);

            var result = userLogic.LoginByUsername("username", "password");

            userMock.VerifyAll();
        }

        [TestMethod]
        public void GetUsersByName()
        {
            userRepositoryMock.Setup(m => m.FindByCondition(user => user.IsStudent && user.Username.Contains("Maria"))).Returns((ICollection<User>)userList);

            var result = userLogic.GetUsers("Maria");

            userRepositoryMock.VerifyAll();

            Assert.AreEqual(2, result.Count());
            Assert.AreEqual("Maria", result.ElementAt(0).Username.ToString());
            Assert.AreEqual("Maria Fernanda", result.ElementAt(1).Username.ToString());
        }

        [TestMethod]
        public void GetAllUsers()
        {
            userRepositoryMock.Setup(m => m.GetAll()).Returns((ICollection<User>)userList);

            var result = userLogic.GetUsers("");

            userRepositoryMock.VerifyAll();

            Assert.AreEqual(2, result.Count());
            Assert.AreEqual("Maria", result.ElementAt(0).Username.ToString());
            Assert.AreEqual("Maria Fernanda", result.ElementAt(1).Username.ToString());
        }
    }
}
