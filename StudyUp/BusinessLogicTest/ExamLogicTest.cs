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
        Group groupExample;
        Mock<IRepository<Exam>> examRepositoryMock;
        Mock<IRepository<User>> userRepositoryMock;
        Mock<IRepository<Group>> groupRepositoryMock;
        Mock<IRepository<ExamCard>> examCardRepositoryMock;
        Mock<IUserRepository> userTokenRepositoryMock;
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

            groupExample = new Group()
            {
                Id = 1,
                Name = "NewGroup",
                Creator = userExample,
                UserGroups = new List<UserGroup>(),
                DeckGroups = new List<DeckGroup>()
            };

            examRepositoryMock = new Mock<IRepository<Exam>>(MockBehavior.Strict);
            userRepositoryMock = new Mock<IRepository<User>>(MockBehavior.Loose);
            examCardRepositoryMock = new Mock<IRepository<ExamCard>>(MockBehavior.Strict);
            groupRepositoryMock = new Mock<IRepository<Group>>(MockBehavior.Strict);
            userTokenRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            examLogic = new ExamLogic(examRepositoryMock.Object, userRepositoryMock.Object,
                 userTokenRepositoryMock.Object, examCardRepositoryMock.Object, groupRepositoryMock.Object);
        }

        [TestMethod]
        public void AddExamOkTest()
        {
            examRepositoryMock.Setup(m => m.Add(It.IsAny<Exam>()));
            examRepositoryMock.Setup(m => m.GetAll()).Returns(new List<Exam>());
            userRepositoryMock.Setup(m => m.GetById(1)).Returns(userExample);
            userRepositoryMock.Setup(m => m.GetById(1)).Returns(userExample);

            userTokenRepositoryMock.Setup(m => m.GetUserByToken(It.IsAny<string>())).Returns(userExample);
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
            userTokenRepositoryMock.Setup(m => m.GetUserByToken(It.IsAny<string>())).Returns(userExample);

            var result = examLogic.GetTeachersExams(userExample.Token).Count();

            examRepositoryMock.VerifyAll();

            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void GetExamByIdTest()
        {
            examRepositoryMock.Setup(b => b.GetById(examExample.Id)).Returns(examExample);
            userTokenRepositoryMock.Setup(m => m.GetUserByToken(It.IsAny<string>())).Returns(userExample);

            var result = examLogic.GetExamById(examExample.Id, userExample.Token);

            examRepositoryMock.VerifyAll();

            Assert.AreEqual(examExample, result);
        }

        [TestMethod]
        public void AssignOkTest()
        {
            userTokenRepositoryMock.Setup(m => m.GetUserByToken(It.IsAny<string>())).Returns(userExample);
            groupRepositoryMock.Setup(m => m.GetById(It.IsAny<int>())).Returns(groupExample);
            examRepositoryMock.Setup(e => e.GetById(It.IsAny<int>())).Returns(examExample);
            examRepositoryMock.Setup(e => e.Update(examExample));

            var result = examLogic.AssignExam(userExample.Token, 1, 1);
            groupRepositoryMock.VerifyAll();
            Assert.AreEqual(examExample, result);
        }
    }
}
