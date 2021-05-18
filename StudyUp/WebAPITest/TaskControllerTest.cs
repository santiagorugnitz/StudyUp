using BusinessLogicInterface;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using WebAPI;
using WebAPI.Controllers;
using WebAPI.Models.ResponseModels;

namespace WebAPITest
{
    [TestClass]
    public class TaskControllerTest
    {
        Mock<IUserLogic> logicMock;
        TasksController controller;

        [TestInitialize]
        public void SetUp()
        {
            logicMock = new Mock<IUserLogic>(MockBehavior.Strict);
            controller = new TasksController(logicMock.Object);
        }

        [TestMethod]
        public void GetTasks()
        {
            logicMock.Setup(x => x.GetTasks(It.IsAny<string>())).Returns(new Tuple<List<Deck>, List<Exam>>(new List<Deck>(), new List<Exam>()));

            var result = controller.GetTasks("token");
            var okResult = result as OkObjectResult;
            var value = okResult.Value as ResponseTasksModel;

            logicMock.VerifyAll();
        }

        [TestMethod]
        public void GetTasksWithLists()
        {
            var deckList = new List<Deck>() { new Deck() { Id = 1, Name = "Name" } };
            var examList = new List<Exam>() { new Exam() { Id = 1, Name = "Name", Group = new Group() { Name = "Group Name"} } };

            logicMock.Setup(x => x.GetTasks(It.IsAny<string>())).Returns(new Tuple<List<Deck>, List<Exam>>(deckList, examList));

            var result = controller.GetTasks("token");
            var okResult = result as OkObjectResult;
            var value = okResult.Value as ResponseTasksModel;

            logicMock.VerifyAll();
        }
    }
}
