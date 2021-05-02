using BusinessLogic;
using DataAccessInterface;
using Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

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
    }
}
