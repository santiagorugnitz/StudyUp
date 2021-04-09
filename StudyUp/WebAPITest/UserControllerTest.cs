using BusinessLogicInterface;
using Domain;
using Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using WebAPI.Controllers;
using WebAPI.Models;

namespace WebAPITest
{
    [TestClass]
    public class UserControllerTest
    {
        Mock<IUserLogic> logicMock;
        UserController controller;
        UserModel userModelExample;

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
        }

        [TestMethod]
        public void PostUserOkTest()
        {
            logicMock.Setup(x => x.AddUser(It.IsAny<User>())).Returns(new User());

            var result = controller.Post(userModelExample);
            var okResult = result as OkObjectResult;
            var value = okResult.Value as User;

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
    }
}
