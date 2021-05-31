using BusinessLogic;
using BusinessLogicInterface;
using DataAccessInterface;
using Domain;
using Domain.Enumerations;
using Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

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
        Mock<INotifications> notificationsInterfaceMock;

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
            notificationsInterfaceMock = new Mock<INotifications>(MockBehavior.Strict);
            examLogic = new ExamLogic(examRepositoryMock.Object, userRepositoryMock.Object,
                 userTokenRepositoryMock.Object, examCardRepositoryMock.Object, groupRepositoryMock.Object,
                 notificationsInterfaceMock.Object);
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

        [ExpectedException(typeof(NotAuthenticatedException))]
        [TestMethod]
        public void AddExamNulllUserTest()
        {
            userTokenRepositoryMock.Setup(m => m.GetUserByToken(It.IsAny<string>())).Returns((User)null);
            
            var result = examLogic.AddExam(examExample, userExample.Token);
        }

        [ExpectedException(typeof(InvalidException))]
        [TestMethod]
        public void AddExamStudentUserTest()
        {
            userExample.IsStudent = true;
            userTokenRepositoryMock.Setup(m => m.GetUserByToken(It.IsAny<string>())).Returns(userExample);
            
            var result = examLogic.AddExam(examExample, userExample.Token);
        }

        [ExpectedException(typeof(AlreadyExistsException))]
        [TestMethod]
        public void AddExamSameNameTest()
        {
            examRepositoryMock.Setup(m => m.GetAll()).Returns(new List<Exam>() { examExample });
            userTokenRepositoryMock.Setup(m => m.GetUserByToken(It.IsAny<string>())).Returns(userExample);

            var result = examLogic.AddExam(examExample, userExample.Token);
        }

        [ExpectedException(typeof(InvalidException))]
        [TestMethod]
        public void AddExamNullNameTest()
        {
            examExample.Name = null;
            examRepositoryMock.Setup(m => m.GetAll()).Returns(new List<Exam>());
            userTokenRepositoryMock.Setup(m => m.GetUserByToken(It.IsAny<string>())).Returns(userExample);

            var result = examLogic.AddExam(examExample, userExample.Token);
        }

        [ExpectedException(typeof(InvalidException))]
        [TestMethod]
        public void AddExamNullSubjectTest()
        {
            examExample.Subject = null;
            examRepositoryMock.Setup(m => m.GetAll()).Returns(new List<Exam>());
            userTokenRepositoryMock.Setup(m => m.GetUserByToken(It.IsAny<string>())).Returns(userExample);

            var result = examLogic.AddExam(examExample, userExample.Token);
        }

        [ExpectedException(typeof(InvalidException))]
        [TestMethod]
        public void AddExamWrongDifficultyTest()
        {
            examExample.Difficulty = (Difficulty)3;
            examRepositoryMock.Setup(m => m.GetAll()).Returns(new List<Exam>());
            userTokenRepositoryMock.Setup(m => m.GetUserByToken(It.IsAny<string>())).Returns(userExample);

            var result = examLogic.AddExam(examExample, userExample.Token);
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
        [ExpectedException(typeof(NotFoundException))]
        public void GetTeachersExamsNullUserTest()
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
            userTokenRepositoryMock.Setup(m => m.GetUserByToken(It.IsAny<string>())).Returns((User)null);

            var result = examLogic.GetTeachersExams(userExample.Token).Count();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidException))]
        public void GetTeachersExamsUserStudentTest()
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

            userExample.IsStudent = true;
            examRepositoryMock.Setup(a => a.FindByCondition(It.IsAny<Expression<Func<Exam,
                bool>>>())).Returns(new List<Exam>() { exam });
            userTokenRepositoryMock.Setup(m => m.GetUserByToken(It.IsAny<string>())).Returns(userExample);

            var result = examLogic.GetTeachersExams(userExample.Token).Count();
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
        [ExpectedException(typeof(NotFoundException))]
        public void GetExamByIdNullExamTest()
        {
            examRepositoryMock.Setup(b => b.GetById(examExample.Id)).Returns((Exam)null);
            userTokenRepositoryMock.Setup(m => m.GetUserByToken(It.IsAny<string>())).Returns(userExample);

            var result = examLogic.GetExamById(examExample.Id, userExample.Token);
        }

        [TestMethod]
        public void GetExamResultsNullResultTest()
        {
            examExample.AlreadyPerformed = new List<UserExam>()
            { new UserExam() { User = userExample, UserId = userExample.Id, Exam = examExample, ExamId = examExample.Id, Score = null } };

            examRepositoryMock.Setup(b => b.GetById(examExample.Id)).Returns(examExample);
            userTokenRepositoryMock.Setup(m => m.GetUserByToken(It.IsAny<string>())).Returns(userExample);

            var result = examLogic.GetResults(examExample.Id, userExample.Token);

            examRepositoryMock.VerifyAll();

            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void GetExamResultsNoResultTest()
        {
            examExample.AlreadyPerformed = new List<UserExam>() { };

            examRepositoryMock.Setup(b => b.GetById(examExample.Id)).Returns(examExample);
            userTokenRepositoryMock.Setup(m => m.GetUserByToken(It.IsAny<string>())).Returns(userExample);

            var result = examLogic.GetResults(examExample.Id, userExample.Token);

            examRepositoryMock.VerifyAll();

            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void GetExamResultTest()
        {
            examExample.AlreadyPerformed = new List<UserExam>()
            { new UserExam() { User = userExample, UserId = userExample.Id, Exam = examExample, ExamId = examExample.Id, Score = 5 } };

            examRepositoryMock.Setup(b => b.GetById(examExample.Id)).Returns(examExample);
            userTokenRepositoryMock.Setup(m => m.GetUserByToken(It.IsAny<string>())).Returns(userExample);

            var result = examLogic.GetResults(examExample.Id, userExample.Token);

            examRepositoryMock.VerifyAll();

            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(userExample.Username, result.ElementAt(0).Item1);
            Assert.AreEqual(5, result.ElementAt(0).Item2);
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public void GetExamResultsNullExamTest()
        {
            examRepositoryMock.Setup(b => b.GetById(examExample.Id)).Returns((Exam)null);
            userTokenRepositoryMock.Setup(m => m.GetUserByToken(It.IsAny<string>())).Returns(userExample);

            var result = examLogic.GetResults(examExample.Id, userExample.Token);

            examRepositoryMock.VerifyAll();

            Assert.AreEqual(examExample, result);
        }


        [TestMethod]
        public void AssignOkTest()
        {
            groupExample.AssignedExams = new List<Exam>();

            userTokenRepositoryMock.Setup(m => m.GetUserByToken(It.IsAny<string>())).Returns(userExample);
            groupRepositoryMock.Setup(m => m.GetById(It.IsAny<int>())).Returns(groupExample);
            groupRepositoryMock.Setup(m => m.Update(It.IsAny<Group>()));
            examRepositoryMock.Setup(e => e.GetById(It.IsAny<int>())).Returns(examExample);
            examCardRepositoryMock.Setup(e => e.FindByCondition(It.IsAny<Expression<Func<ExamCard, bool>>>())).Returns(new List<ExamCard>() { new ExamCard() { Exam = examExample } });
            examRepositoryMock.Setup(e => e.Update(examExample));
            notificationsInterfaceMock.Setup(e => e.NotifyExams(It.IsAny<Exam>(), It.IsAny<Group>()));

            var result = examLogic.AssignExam(userExample.Token, 1, 1);
            groupRepositoryMock.VerifyAll();
            Assert.AreEqual(examExample, result);
        }

        [ExpectedException(typeof(NotAuthenticatedException))]
        [TestMethod]
        public void AssignNullUserTest()
        {
            groupExample.AssignedExams = new List<Exam>();

            userTokenRepositoryMock.Setup(m => m.GetUserByToken(It.IsAny<string>())).Returns((User)null);
            groupRepositoryMock.Setup(m => m.GetById(It.IsAny<int>())).Returns(groupExample);
            examRepositoryMock.Setup(e => e.GetById(It.IsAny<int>())).Returns(examExample);
            
            var result = examLogic.AssignExam(userExample.Token, 1, 1);
        }

        [ExpectedException(typeof(NotFoundException))]
        [TestMethod]
        public void AssignNullGroupTest()
        {
            groupExample.AssignedExams = new List<Exam>();

            userTokenRepositoryMock.Setup(m => m.GetUserByToken(It.IsAny<string>())).Returns(userExample);
            groupRepositoryMock.Setup(m => m.GetById(It.IsAny<int>())).Returns((Group)null);
            examRepositoryMock.Setup(e => e.GetById(It.IsAny<int>())).Returns(examExample);

            var result = examLogic.AssignExam(userExample.Token, 1, 1);
        }

        [ExpectedException(typeof(NotFoundException))]
        [TestMethod]
        public void AssignNullExamTest()
        {
            groupExample.AssignedExams = new List<Exam>();

            userTokenRepositoryMock.Setup(m => m.GetUserByToken(It.IsAny<string>())).Returns(userExample);
            groupRepositoryMock.Setup(m => m.GetById(It.IsAny<int>())).Returns(groupExample);
            examRepositoryMock.Setup(e => e.GetById(It.IsAny<int>())).Returns((Exam)null);

            var result = examLogic.AssignExam(userExample.Token, 1, 1);
        }

        [ExpectedException(typeof(AlreadyExistsException))]
        [TestMethod]
        public void AssignNullExamGroupTest()
        {
            examExample.Group = new Group();

            userTokenRepositoryMock.Setup(m => m.GetUserByToken(It.IsAny<string>())).Returns(userExample);
            groupRepositoryMock.Setup(m => m.GetById(It.IsAny<int>())).Returns(groupExample);
            examRepositoryMock.Setup(e => e.GetById(It.IsAny<int>())).Returns(examExample);

            var result = examLogic.AssignExam(userExample.Token, 1, 1);
        }

        [ExpectedException(typeof(InvalidException))]
        [TestMethod]
        public void AssignWrongCreatorTest()
        {
            groupExample.Creator = new User() { Email = "another email" };

            userTokenRepositoryMock.Setup(m => m.GetUserByToken(It.IsAny<string>())).Returns(userExample);
            groupRepositoryMock.Setup(m => m.GetById(It.IsAny<int>())).Returns(groupExample);
            examRepositoryMock.Setup(e => e.GetById(It.IsAny<int>())).Returns(examExample);

            var result = examLogic.AssignExam(userExample.Token, 1, 1);
        }

        [TestMethod]
        public void AssignExamResultsNullResultTest()
        {
            examExample.AlreadyPerformed = new List<UserExam> { new UserExam()
            { User = userExample, UserId = userExample.Id, Exam = examExample, ExamId = examExample.Id, Score = null} };
            examExample.ExamCards = new List<ExamCard>() { new ExamCard() };
            examRepositoryMock.Setup(b => b.Update(It.IsAny<Exam>()));
            examRepositoryMock.Setup(b => b.GetById(examExample.Id)).Returns(examExample);
            userTokenRepositoryMock.Setup(m => m.GetUserByToken(It.IsAny<string>())).Returns(userExample);
            examLogic.AssignResults(examExample.Id, userExample.Token, 60, 10);

            examRepositoryMock.VerifyAll();
        }

        [TestMethod]
        public void AssignExamResultsNoResultTest()
        {
            examExample.AlreadyPerformed = new List<UserExam> { };
            examExample.ExamCards = new List<ExamCard>();
            examRepositoryMock.Setup(b => b.Update(It.IsAny<Exam>()));
            examRepositoryMock.Setup(b => b.GetById(examExample.Id)).Returns(examExample);
            userTokenRepositoryMock.Setup(m => m.GetUserByToken(It.IsAny<string>())).Returns(userExample);
            examLogic.AssignResults(examExample.Id, userExample.Token, 60, 10);

            examRepositoryMock.VerifyAll();
        }

        [TestMethod]
        public void AssignExamResultZeroQuestionsTest()
        {
            examExample.AlreadyPerformed = new List<UserExam> { new UserExam()
            { User = userExample, UserId = userExample.Id, Exam = examExample, ExamId = examExample.Id, Score = null} };
            examExample.ExamCards = new List<ExamCard>();
            examRepositoryMock.Setup(b => b.Update(It.IsAny<Exam>()));
            examRepositoryMock.Setup(b => b.GetById(examExample.Id)).Returns(examExample);
            userTokenRepositoryMock.Setup(m => m.GetUserByToken(It.IsAny<string>())).Returns(userExample);
            examLogic.AssignResults(examExample.Id, userExample.Token, 60, 10);

            examRepositoryMock.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidException))]
        public void AssignExamResultsAlreadyDoneTest()
        {
            examExample.AlreadyPerformed = new List<UserExam> { new UserExam() 
            { User = userExample, UserId = userExample.Id, Exam = examExample, ExamId = examExample.Id, Score = 5} };
            examRepositoryMock.Setup(b => b.Update(It.IsAny<Exam>()));
            examRepositoryMock.Setup(b => b.GetById(examExample.Id)).Returns(examExample);
            userTokenRepositoryMock.Setup(m => m.GetUserByToken(It.IsAny<string>())).Returns(userExample);
            examLogic.AssignResults(examExample.Id, userExample.Token, 60, 10);

            examRepositoryMock.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public void AssignExamResultsNullExamTest()
        {
            examRepositoryMock.Setup(b => b.Update(It.IsAny<Exam>()));
            examRepositoryMock.Setup(b => b.GetById(examExample.Id)).Returns((Exam) null);
            userTokenRepositoryMock.Setup(m => m.GetUserByToken(It.IsAny<string>())).Returns(userExample);
            examLogic.AssignResults(examExample.Id, userExample.Token, 60, 10);

            examRepositoryMock.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(NotAuthenticatedException))]
        public void AssignExamResultsNullUserTest()
        {
            examRepositoryMock.Setup(b => b.Update(It.IsAny<Exam>()));
            examRepositoryMock.Setup(b => b.GetById(examExample.Id)).Returns(examExample);
            userTokenRepositoryMock.Setup(m => m.GetUserByToken(It.IsAny<string>())).Returns((User)null);
            examLogic.AssignResults(examExample.Id, userExample.Token, 60, 10);

            examRepositoryMock.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void AssignExamResultsNullReferenceAlreadyPerformedTest()
        {
            examExample.AlreadyPerformed = null;

            examRepositoryMock.Setup(b => b.Update(It.IsAny<Exam>()));
            examRepositoryMock.Setup(b => b.GetById(examExample.Id)).Returns(examExample);
            userTokenRepositoryMock.Setup(m => m.GetUserByToken(It.IsAny<string>())).Returns(userExample);
            examLogic.AssignResults(examExample.Id, userExample.Token, 60, 10);

            examRepositoryMock.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidException))]
        public void AssignExamResultsNoExamCards()
        {
            groupExample.AssignedExams = new List<Exam>();

            userTokenRepositoryMock.Setup(m => m.GetUserByToken(It.IsAny<string>())).Returns(userExample);
            groupRepositoryMock.Setup(m => m.GetById(It.IsAny<int>())).Returns(groupExample);
            groupRepositoryMock.Setup(m => m.Update(It.IsAny<Group>()));
            examRepositoryMock.Setup(e => e.GetById(It.IsAny<int>())).Returns(examExample);
            examCardRepositoryMock.Setup(e => e.FindByCondition(It.IsAny<Expression<Func<ExamCard, bool>>>())).Returns(new List<ExamCard>() { });
            examRepositoryMock.Setup(e => e.Update(examExample));
            notificationsInterfaceMock.Setup(e => e.NotifyExams(It.IsAny<Exam>(), It.IsAny<Group>()));

            var result = examLogic.AssignExam(userExample.Token, 1, 1);
        }
    }
}
