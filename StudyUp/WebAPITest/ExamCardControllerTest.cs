using BusinessLogicInterface;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebAPI.Controllers;
using WebAPI.Models;
using WebAPI.Models.RequestModels;

namespace WebAPITest
{
    [TestClass]
    public class ExamCardControllerTest
    {
        Mock<IExamCardLogic> logicMock;
        ExamCardController controller;
        ExamCardModel examCardModelExample;
        UserModel userModelExample;
        ExamCard examCard;

        [TestInitialize]
        public void SetUp()
        {
            logicMock = new Mock<IExamCardLogic>(MockBehavior.Strict);
            controller = new ExamCardController(logicMock.Object);

            userModelExample = new UserModel()
            {
                Id = 1,
                Username = "Raul",
                IsStudent = false,
                Email = "raul@hotmail.com",
                Password = "Contraseña123",
                Token = "token"
            };

            examCardModelExample = new ExamCardModel()
            {
                Question = "exam card question",
                Answer = true,
                ExamId = 1
            };

            examCard = new ExamCard
            {
                Id = 1,
                Question = "exam card question",
                Answer = true,
                Exam = new Exam() { }
            };
        }

        [TestMethod]
        public void PostExamCardOkTest()
        {
            logicMock.Setup(x => x.AddExamCard(1, It.IsAny<ExamCard>(), It.IsAny<string>())).Returns(examCard);

            var result = controller.Post(examCardModelExample, userModelExample.Token);
            var okResult = result as OkObjectResult;

            logicMock.VerifyAll();
        }

        [TestMethod]
        public void DeleteExamCardOkTest()
        {
            logicMock.Setup(x => x.DeleteExamCard(It.IsAny<int>(), It.IsAny<string>())).Returns(true);

            var result = controller.Delete(examCard.Id, userModelExample.Token);
            var okResult = result as OkObjectResult;

            logicMock.VerifyAll();
        }

        [TestMethod]
        public void EditExamCardOkTest()
        {
            EditExamCardModel editExamCardModel = new EditExamCardModel()
            {
                Answer = false,
                Question = "new question"
            };

            logicMock.Setup(x => x.EditExamCard(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(),
                It.IsAny<bool>())).Returns(new ExamCardModel().ToEntity());

            var result = controller.EditExamCard(1, "token", editExamCardModel);
            var okResult = result as OkObjectResult;
            var value = okResult.Value as ExamCard;

            logicMock.VerifyAll();
        }
    }
}
