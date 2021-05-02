using BusinessLogic;
using DataAccessInterface;
using Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace BusinessLogicTest
{
    [TestClass]
    public class ExamLogicTest
    {
        User userExample;
        Exam examExample;
        Mock<IRepository<Exam>> examRepositoryMock;
        Mock<IRepository<User>> userRepositoryMock;
        Mock<IRepository<ExamCard>> examCardRepositoryMock;
        Mock<IUserRepository> userTokenRepository;
        ExamLogic examLogic;

        [TestInitialize]
        public void SetUp()
        {
            userExample = new User()
            {
                Id = 1,
                Username = "Ana",
                Email = "rosa@gmail.com",
                Password = "Rosa1234",
                IsStudent = false,
                Token = "token",
                Decks = new List<Deck>(),
                Exams = new List<Exam>()
            };

            examExample = new Exam()
            {
                Id = 1,
                Name = "Exam1",
                Author = userExample,
                Difficulty = Domain.Enumerations.Difficulty.Medium,
                Subject = "English",
                ExamCards = new List<ExamCard>()
            };

            examRepositoryMock = new Mock<IRepository<Exam>>(MockBehavior.Strict);
            userRepositoryMock = new Mock<IRepository<User>>(MockBehavior.Loose);
            examCardRepositoryMock = new Mock<IRepository<ExamCard>>(MockBehavior.Strict);
            userTokenRepository = new Mock<IUserRepository>(MockBehavior.Strict);
            examLogic = new ExamLogic(examRepositoryMock.Object, userRepositoryMock.Object,
                 userTokenRepository.Object, examCardRepositoryMock.Object);
        }

        [TestMethod]
        public void AddExamOkTest()
        {
            examRepositoryMock.Setup(m => m.Add(It.IsAny<Exam>()));
            examRepositoryMock.Setup(m => m.GetAll()).Returns(new List<Exam>());
            userRepositoryMock.Setup(m => m.GetById(1)).Returns(userExample);
            userTokenRepository.Setup(m => m.GetUserByToken(It.IsAny<string>())).Returns(userExample);
            userRepositoryMock.Setup(a => a.Update(It.IsAny<User>()));

            userRepositoryMock.Setup(m => m.Add(It.IsAny<User>()));

            var result = examLogic.AddExam(examExample, userExample.Token);

            examRepositoryMock.VerifyAll();
            Assert.AreEqual(examExample, result);
        }

        [TestMethod]
        public void GetTeachersExamsTest()
        {
            Exam exam = new Exam
            {
                Id = 1,
                Name = "German Exam",
                Author = userExample,
                Difficulty = Domain.Enumerations.Difficulty.Hard,
                Subject = "German",
                ExamCards = new List<ExamCard>(),
            };

            examRepositoryMock.Setup(a => a.FindByCondition(It.IsAny<Expression<Func<Exam,
                bool>>>())).Returns(new List<Exam>() { exam });
            userTokenRepository.Setup(m => m.GetUserByToken(It.IsAny<string>())).Returns(userExample);

            var result = examLogic.GetTeachersExams(userExample.Token).Count();

            examRepositoryMock.VerifyAll();

            Assert.AreEqual(1, result);
        }
    }
}
