﻿using BusinessLogicInterface;
using Domain;
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

    }
}
