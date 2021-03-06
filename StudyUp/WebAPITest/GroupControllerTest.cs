using BusinessLogicInterface;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
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

            logicMock.VerifyAll();
        }

        [TestMethod]
        public void SubscribeOkTest()
        {
            logicMock.Setup(x => x.Subscribe(It.IsAny<string>(), It.IsAny<int>())).Returns(true);

            var result = controller.Subscribe("token", 1);
            var okResult = result as OkObjectResult;

            logicMock.VerifyAll();
        }

        [TestMethod]
        public void UnsubscribeOkTest()
        {
            logicMock.Setup(x => x.Unsubscribe(It.IsAny<string>(), It.IsAny<int>())).Returns(true);

            var result = controller.Unsubscribe("token", 1);
            var okResult = result as OkObjectResult;

            logicMock.VerifyAll();
        }

        [TestMethod]
        public void GetGroupsOkTest()
        {
            logicMock.Setup(x => x.GetAllGroups(It.IsAny<string>())).Returns(new List<Group>() { groupModelExample.ToEntity() });

            var result = controller.Get("token", groupModelExample.Name);
            var okResult = result as OkObjectResult;
            var value = okResult.Value as List<ResponseGroupModel>;

            logicMock.VerifyAll();
        }

        [TestMethod]
        public void GetGroupsWithoutNameOkTest()
        {
            logicMock.Setup(x => x.GetTeachersGroups(It.IsAny<string>())).Returns(new List<Group>() { groupModelExample.ToEntity() });
            logicMock.Setup(x => x.GetGroupDecks(It.IsAny<int>())).Returns(new List<Deck>() { new Deck() { Id = 1, Name = "name" } });

            var result = controller.Get("token");
            var okResult = result as OkObjectResult;
            var value = okResult.Value as List<ResponseGroupModel>;

            logicMock.VerifyAll();
        }
    }
}
