using BusinessLogicInterface;
using Domain;
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
                Name = "Jose",
                IsATeacher = false,
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
        public void PostUserAlreadyExistsTest()
        {
            logicMock.Setup(x => x.AddUser(It.IsAny<User>())).Throws(new Exception());

            var result = controller.Post(userModelExample);
            var okResult = result as BadRequestObjectResult;

            logicMock.VerifyAll();
        }
    }
}
