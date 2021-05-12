using BusinessLogicInterface;
using Domain;
using Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using WebAPI.Controllers;
using WebAPI.Models;
using WebAPI.Models.ResponseModels;

namespace WebAPITest
{
    [TestClass]
    public class UserControllerTest
    {
        Mock<IUserLogic> logicMock;
        UserController controller;
        UserModel userModelExample;
        LoginModel loginModelExample;

        [TestInitialize]
        public void SetUp()
        {
            logicMock = new Mock<IUserLogic>(MockBehavior.Strict);
            controller = new UserController(logicMock.Object);

            userModelExample = new UserModel()
            {
                Username = "Jose",
                IsStudent = false,
                Email = "jose@hotmail.com",
                Password = "contraseña123",
            };

            loginModelExample = new LoginModel()
            {
                Email = "jose@hotmail.com",
                Password = "contraseña123"
            };
        }

        [TestMethod]
        public void PostUserOkTest()
        {
            logicMock.Setup(x => x.AddUser(It.IsAny<User>())).Returns(new User());

            var result = controller.Post(userModelExample);
            var okResult = result as OkObjectResult;
            var value = okResult.Value as ResponseUserModel;

            logicMock.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(AlreadyExistsException))]
        public void PostUserAlreadyExistsTest()
        {
            logicMock.Setup(x => x.AddUser(It.IsAny<User>())).Throws(new AlreadyExistsException(UserMessage.EMAIL_ALREADY_EXISTS));

            var result = controller.Post(userModelExample);
            var okResult = result as BadRequestObjectResult;

            logicMock.VerifyAll();
        }

        [TestMethod]
        public void LoginOkTest()
        {
            logicMock.Setup(x => x.Login(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(new User());

            var result = controller.Login(loginModelExample);
            var okResult = result as OkObjectResult;
            var value = okResult.Value as ResponseUserModel;

            logicMock.VerifyAll();
        }

        [TestMethod]
        public void LoginWithUsernameTest()
        {
            logicMock.Setup(x => x.LoginByUsername(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(new User());
            loginModelExample.Username = "username";

            var result = controller.Login(loginModelExample);
            var okResult = result as OkObjectResult;
            var value = okResult.Value as ResponseUserModel;

            logicMock.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidException))]
        public void LoginCredentialsAreWrongTest()
        {
            logicMock.Setup(x => x.Login(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Throws(new InvalidException(UserMessage.USER_NOT_FOUND));

            var result = controller.Login(loginModelExample);
            var okResult = result as BadRequestObjectResult;

            logicMock.VerifyAll();
        }

        [TestMethod]
        public void GetUsersOkWithParam()
        {
            logicMock.Setup(x => x.GetUsers(It.IsAny<string>(), It.IsAny<string>())).Returns(new List<Tuple<User, bool>>());

            var result = controller.GetUsers("Token", "Name");
            var okResult = result as OkObjectResult;
            var value = okResult.Value as IEnumerable<ResponseFollowedUserModel>;

            logicMock.VerifyAll();
        }

        [TestMethod]
        public void GetUsersOkWithNullParam()
        {
            logicMock.Setup(x => x.GetUsers(It.IsAny<string>(), It.IsAny<string>())).Returns(new List<Tuple<User, bool>>());

            var result = controller.GetUsers("Token", null);
            var okResult = result as OkObjectResult;
            var value = okResult.Value as IEnumerable<ResponseFollowedUserModel>;

            logicMock.VerifyAll();
        }

        [TestMethod]
        public void GetUsersOkWithoutParam()
        {
            logicMock.Setup(x => x.GetUsers(It.IsAny<string>(), It.IsAny<string>())).Returns(new List<Tuple<User, bool>>());

            var result = controller.GetUsers("Token");
            var okResult = result as OkObjectResult;
            var value = okResult.Value as IEnumerable<ResponseFollowedUserModel>;

            logicMock.VerifyAll();
        }

        [TestMethod]
        public void FollowUserOk()
        {
            logicMock.Setup(x => x.FollowUser(It.IsAny<string>(), It.IsAny<string>())).Returns(new User());

            var result = controller.FollowUser("token", "Name");
            var okResult = result as OkObjectResult;
            var value = okResult.Value as User;

            logicMock.VerifyAll();
        }

        [TestMethod]
        public void UnfollowUserOk()
        {
            logicMock.Setup(x => x.UnfollowUser(It.IsAny<string>(), It.IsAny<string>())).Returns(new User());

            var result = controller.UnfollowUser("token", "Name");
            var okResult = result as OkObjectResult;
            var value = okResult.Value as User;

            logicMock.VerifyAll();
        }

        [TestMethod]
        public void GetDecksOk()
        {
            logicMock.Setup(x => x.GetDecksFromFollowing(It.IsAny<string>())).Returns(new List<Deck>());

            var result = controller.GetDecks("token");
            var okResult = result as OkObjectResult;
            var value = okResult.Value as IEnumerable<ResponseDeckModel>;

            logicMock.VerifyAll();
        }

        [TestMethod]
        public void RankingOk()
        {
            logicMock.Setup(x => x.GetUsersForRanking(It.IsAny<string>())).Returns(It.IsAny<List<User>>());

            var result = controller.Ranking("token");
            var okResult = result as OkObjectResult;
            var value = okResult.Value as IEnumerable<ResponseRankingModel>;

            logicMock.VerifyAll();
        }
    }
}
