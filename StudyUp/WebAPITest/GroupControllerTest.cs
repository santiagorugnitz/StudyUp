using BusinessLogicInterface;
using Domain;
using Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using WebAPI.Controllers;
using WebAPI.Models;

namespace WebAPITest
{
    [TestClass]
    public class GroupControllerTest
    {
        Mock<IGroupLogic> logicMock;
        GroupController controller;
        GroupModel groupModelExample;
        UserModel userModelExample;

        [TestInitialize]
        public void SetUp()
        {
            logicMock = new Mock<IGroupLogic>(MockBehavior.Strict);
            controller = new GroupController(logicMock.Object);


            userModelExample = new UserModel()
            {
                Id = 1,
                Username = "Jose",
                IsStudent = false,
                Email = "jose@hotmail.com",
                Password = "contraseña123",
            };

            groupModelExample = new GroupModel()
            {
                Name = "New Group"
            };
        }

        [TestMethod]
        public void PostGroupOkTest()
        {
            logicMock.Setup(x => x.AddGroup(It.IsAny<Group>(), It.IsAny<string>())).Returns(new Group());

            var result = controller.Post(groupModelExample, "token");
            var okResult = result as OkObjectResult;
            var value = okResult.Value as User;

            logicMock.VerifyAll();
        }
    }
}
