using BusinessLogic;
using DataAccessInterface;
using Domain;
using Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;

namespace BusinessLogicTest
{
    [TestClass]
    public class ExamCardLogicTest
    {
        User userExample;
        Exam examExample;
        ExamCard examCardExample;
        Mock<IRepository<Exam>> examRepositoryMock;
        Mock<IRepository<User>> userRepositoryMock;
        Mock<IRepository<ExamCard>> examCardRepositoryMock;
        Mock<IUserRepository> userTokenRepository;
        ExamCardLogic examCardLogic;

        [TestInitialize]
        public void SetUp()
        {
            userExample = new User()
            {
                Id = 1,
                Username = "Gabriel",
                Email = "gabriel@gmail.com",
                Password = "Gabriel1234",
                IsStudent = false,
                Token = "token",
                Decks = new List<Deck>(),
                Exams = new List<Exam>()
            };

            examExample = new Exam()
            {
                Id = 1,
                Name = "PEExam",
                Author = userExample,
                Difficulty = Domain.Enumerations.Difficulty.Medium,
                Subject = "PE",
                ExamCards = new List<ExamCard>()
            };

            examCardExample = new ExamCard()
            {
                Id = 1,
                Question = "This is a question",
                Answer = false,
                Exam = examExample
            };

            examRepositoryMock = new Mock<IRepository<Exam>>(MockBehavior.Strict);
            userRepositoryMock = new Mock<IRepository<User>>(MockBehavior.Loose);
            examCardRepositoryMock = new Mock<IRepository<ExamCard>>(MockBehavior.Strict);
            userTokenRepository = new Mock<IUserRepository>(MockBehavior.Strict);
            examCardLogic = new ExamCardLogic(examRepositoryMock.Object, userRepositoryMock.Object,
                 userTokenRepository.Object, examCardRepositoryMock.Object);
        }

        [TestMethod]
        public void AddExamCardOkTest()
        {
            examCardRepositoryMock.Setup(m => m.Add(It.IsAny<ExamCard>()));
            examCardRepositoryMock.Setup(m => m.GetAll()).Returns(new List<ExamCard>());
            examRepositoryMock.Setup(m => m.GetById(1)).Returns(examExample);
            userTokenRepository.Setup(m => m.GetUserByToken(It.IsAny<string>())).Returns(userExample);
            examRepositoryMock.Setup(a => a.Update(It.IsAny<Exam>()));

            var result = examCardLogic.AddExamCard(examExample.Id, examCardExample, userExample.Token);

            examRepositoryMock.VerifyAll();
            Assert.AreEqual(examCardExample, result);
        }

        [ExpectedException(typeof(NotAuthenticatedException))]
        [TestMethod]
        public void AddExamCardNullUserTest()
        {
            userTokenRepository.Setup(m => m.GetUserByToken(It.IsAny<string>())).Returns((User)null);
         
            var result = examCardLogic.AddExamCard(examExample.Id, examCardExample, userExample.Token);
        }

        [ExpectedException(typeof(InvalidException))]
        [TestMethod]
        public void AddExamCardUserIsStudentTest()
        {
            userExample.IsStudent = true;

            userTokenRepository.Setup(m => m.GetUserByToken(It.IsAny<string>())).Returns(userExample);
            var result = examCardLogic.AddExamCard(examExample.Id, examCardExample, userExample.Token);
        }

        [ExpectedException(typeof(NotFoundException))]
        [TestMethod]
        public void AddExamCardNullExamTest()
        {
            examRepositoryMock.Setup(m => m.GetById(1)).Returns((Exam)null);
            userTokenRepository.Setup(m => m.GetUserByToken(It.IsAny<string>())).Returns(userExample);
         
            var result = examCardLogic.AddExamCard(examExample.Id, examCardExample, userExample.Token);
        }

        [ExpectedException(typeof(InvalidException))]
        [TestMethod]
        public void AddExamCardWrongAuthorTest()
        {
            examExample.Author = new User() { Email = "new" };

            examRepositoryMock.Setup(m => m.GetById(1)).Returns(examExample);
            userTokenRepository.Setup(m => m.GetUserByToken(It.IsAny<string>())).Returns(userExample);
            var result = examCardLogic.AddExamCard(examExample.Id, examCardExample, userExample.Token);
        }

        [ExpectedException(typeof(AlreadyExistsException))]
        [TestMethod]
        public void AddExamCardAlreadyExistsQuestionTest()
        {
            examCardRepositoryMock.Setup(m => m.GetAll()).Returns(new List<ExamCard>() { examCardExample });
            examRepositoryMock.Setup(m => m.GetById(1)).Returns(examExample);
            userTokenRepository.Setup(m => m.GetUserByToken(It.IsAny<string>())).Returns(userExample);

            var result = examCardLogic.AddExamCard(examExample.Id, examCardExample, userExample.Token);
        }

        [TestMethod]
        public void DeleteExamCardOkTest()
        {
            examCardRepositoryMock.Setup(b => b.GetById(examCardExample.Id)).Returns(examCardExample);
            userRepositoryMock.Setup(b => b.GetById(userExample.Id)).Returns(userExample);
            examRepositoryMock.Setup(d => d.Update(examExample));

            var result = examCardLogic.DeleteExamCard(examCardExample.Id, userExample.Token);

            examCardRepositoryMock.VerifyAll();
            Assert.IsTrue(result);
        }

        [ExpectedException(typeof(NotFoundException))]
        [TestMethod]
        public void DeleteExamNoCardTest()
        {
            examCardRepositoryMock.Setup(b => b.GetById(examCardExample.Id)).Returns((ExamCard) null);

            var result = examCardLogic.DeleteExamCard(examCardExample.Id, userExample.Token);
        }

        [ExpectedException(typeof(InvalidException))]
        [TestMethod]
        public void DeleteExamInvalidAuthorTest()
        {
            examCardRepositoryMock.Setup(b => b.GetById(examCardExample.Id)).Returns(examCardExample);
            userRepositoryMock.Setup(b => b.GetById(userExample.Id)).Returns(userExample);
            examRepositoryMock.Setup(d => d.Update(examExample));

            var result = examCardLogic.DeleteExamCard(examCardExample.Id, "Another token");
        }

        [TestMethod]
        public void EditExamCardOkTest()
        {
            ExamCard examCardAfterEdit = new ExamCard()
            {
                Id = examCardExample.Id,
                Answer = true,
                Question = "new question",
                Exam = examExample
            };

            examCardRepositoryMock.Setup(m => m.GetById(1)).Returns(examCardAfterEdit);
            examCardRepositoryMock.Setup(f => f.Update(It.IsAny<ExamCard>()));

            userTokenRepository.Setup(u => u.GetUserByToken(It.IsAny<string>())).Returns(userExample);

            var result = examCardLogic.EditExamCard("token", 1, "new question", false);
        }

        [ExpectedException(typeof(NotFoundException))]
        [TestMethod]
        public void EditExamCardUserNullTest()
        {
            ExamCard examCardAfterEdit = new ExamCard()
            {
                Id = examCardExample.Id,
                Answer = true,
                Question = "new question",
                Exam = examExample
            };

            examCardRepositoryMock.Setup(m => m.GetById(1)).Returns(examCardAfterEdit);

            userTokenRepository.Setup(u => u.GetUserByToken(It.IsAny<string>())).Returns((User)null);

            var result = examCardLogic.EditExamCard("token", 1, "new question", false);
        }

        [ExpectedException(typeof(NotFoundException))]
        [TestMethod]
        public void EditExamCardExamCardNullTest()
        {
            ExamCard examCardAfterEdit = new ExamCard()
            {
                Id = examCardExample.Id,
                Answer = true,
                Question = "new question",
                Exam = examExample
            };

            examCardRepositoryMock.Setup(m => m.GetById(1)).Returns((ExamCard)null);

            userTokenRepository.Setup(u => u.GetUserByToken(It.IsAny<string>())).Returns(userExample);

            var result = examCardLogic.EditExamCard("token", 1, "new question", false);
        }

        [ExpectedException(typeof(InvalidException))]
        [TestMethod]
        public void EditExamCardWrongAuthorTest()
        {
            ExamCard examCardAfterEdit = new ExamCard()
            {
                Id = examCardExample.Id,
                Answer = true,
                Question = "new question",
                Exam = new Exam() { Author = new User() { Id = 100 } }
            };

            examCardRepositoryMock.Setup(m => m.GetById(1)).Returns(examCardAfterEdit);

            userTokenRepository.Setup(u => u.GetUserByToken(It.IsAny<string>())).Returns(userExample);

            var result = examCardLogic.EditExamCard("token", 1, "new question", false);
        }
    }
}
