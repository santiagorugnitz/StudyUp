using BusinessLogicInterface;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using WebAPI.Controllers;
using WebAPI.Models;
using WebAPI.Models.RequestModels;

namespace WebAPITest
{
    [TestClass]
    public class ExamControllerTest
    {
        Mock<IExamLogic> logicMock;
        ExamController controller;
        ExamModel examModelExample;
        UserModel userModelExample;
        Exam exam;

        [TestInitialize]
        public void SetUp()
        {
            logicMock = new Mock<IExamLogic>(MockBehavior.Strict);
            controller = new ExamController(logicMock.Object);

            userModelExample = new UserModel()
            {
                Id = 1,
                Username = "Sandra",
                IsStudent = false,
                Email = "sandra@hotmail.com",
                Password = "Contraseña123",
                Token = "token"
            };

            examModelExample = new ExamModel()
            {
                Name = "Exam2",
                Difficulty = Domain.Enumerations.Difficulty.Easy,
                Subject = "Japanese"
            };

            exam = new Exam
            {
                Id = 1,
                Author = new User { Username = "" },
                Difficulty = Domain.Enumerations.Difficulty.Easy,
                Name = "",
                Subject = "",
                ExamCards = new List<ExamCard>(),
                Group = new Group() { }
            };
        }

        [TestMethod]
        public void PostDeckOkTest()
        {
            logicMock.Setup(x => x.AddExam(It.IsAny<Exam>(), It.IsAny<string>())).Returns(exam);

            var result = controller.Post(examModelExample, userModelExample.Token);
            var okResult = result as OkObjectResult;

            logicMock.VerifyAll();
        }

        [TestMethod]
        public void GetExamsOkTest()
        {
            logicMock.Setup(x => x.GetTeachersExams(userModelExample.Token)).Returns(new List<Exam>());

            var result = controller.GetTeachersExams(userModelExample.Token);
            var okResult = result as OkObjectResult;
            var value = okResult.Value as List<Deck>;

            logicMock.VerifyAll();
        }

        [TestMethod]
        public void GetExamsOkWithDataTest()
        {
            logicMock.Setup(x => x.GetTeachersExams(userModelExample.Token)).Returns(new List<Exam>() { new Exam() {Id = 1, Name = "name", Group = null },
            new Exam() {Id = 1, Name = "name", Group = new Group() { Name = "name"} }});

            var result = controller.GetTeachersExams(userModelExample.Token);
            var okResult = result as OkObjectResult;
            var value = okResult.Value as List<Deck>;

            logicMock.VerifyAll();
        }

        [TestMethod]
        public void GetExamByIdOkTest()
        {
            var exam = examModelExample.ToEntity();
            exam.ExamCards = new List<ExamCard>();
            exam.Author = new User { Username = "test" };
            logicMock.Setup(x => x.GetExamById(1, "token")).Returns(exam);

            var result = controller.GetExamById(1, "token");
            var okResult = result as OkObjectResult;
            var value = okResult.Value as ResponseFullDeckModel;

            logicMock.VerifyAll();
        }

        [TestMethod]
        public void GetExamByIdNoGroupTest()
        {
            var exam = examModelExample.ToEntity();
            exam.Group = null;
            exam.ExamCards = new List<ExamCard>() { new ExamCard() { Answer = true, Question = "question", Id = 1 } };
            exam.Author = new User { Username = "test" };
            logicMock.Setup(x => x.GetExamById(1, "token")).Returns(exam);

            var result = controller.GetExamById(1, "token");
            var okResult = result as OkObjectResult;
            var value = okResult.Value as ResponseFullDeckModel;

            logicMock.VerifyAll();
        }

        [TestMethod]
        public void GetExamByIdOkNullGroupTest()
        {
            var exam = examModelExample.ToEntity();
            exam.Group = null;
            exam.ExamCards = new List<ExamCard>();
            exam.Author = new User { Username = "test" };
            logicMock.Setup(x => x.GetExamById(1, "token")).Returns(exam);

            var result = controller.GetExamById(1, "token");
            var okResult = result as OkObjectResult;
            var value = okResult.Value as ResponseFullDeckModel;

            logicMock.VerifyAll();
        }

        [TestMethod]
        public void AssingOkTest()
        {
            logicMock.Setup(x => x.AssignExam(It.IsAny<string>(), It.IsAny<int>(),
                It.IsAny<int>())).Returns(It.IsAny<Exam>());

            var result = controller.Assign("token", 1, 1);
            var okResult = result as OkObjectResult;
            var value = okResult.Value as Exam;

            logicMock.VerifyAll();
        }

        [TestMethod]
        public void GetResultsOkTest()
        {
            logicMock.Setup(x => x.GetResults(It.IsAny<int>(), It.IsAny<string>())).Returns(new List<Tuple<string, double>>());

            var result = controller.GetResults(1, "token");
            var okResult = result as OkObjectResult;
            var value = okResult.Value as List<Tuple<string, double>>;

            logicMock.VerifyAll();
        }

        [TestMethod]
        public void GetResultsWithDataOkTest()
        {
            logicMock.Setup(x => x.GetResults(It.IsAny<int>(), It.IsAny<string>())).Returns(new List<Tuple<string, double>>() 
            { new Tuple<string, double>("username", 1.0)});

            var result = controller.GetResults(1, "token");
            var okResult = result as OkObjectResult;
            var value = okResult.Value as List<Tuple<string, double>>;

            logicMock.VerifyAll();
        }

        [TestMethod]
        public void AssignResultsOkTest()
        {
            logicMock.Setup(x => x.AssignResults(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()));

            var result = controller.AssignResults(1, "token", new ScoreModel() { Time = 60, CorrectAnswers = 5 });
            var okResult = result as OkObjectResult;

            logicMock.VerifyAll();
        }
    }
}
