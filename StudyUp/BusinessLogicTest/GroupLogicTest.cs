using BusinessLogic;
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
using System.Text;

namespace BusinessLogicTest
{
    [TestClass]
    public class GroupLogicTest
    {
        User userExample;
        User userTeacherExample;
        Group groupExample;
        Deck deckExample;
        UserGroup userGroupExample;
        Mock<IRepository<Group>> groupRepositoryMock;
        Mock<IUserRepository> userTokenRepositoryMock;
        Mock<IRepository<User>> userRepositoryMock;
        Mock<IRepository<Deck>> deckRepositoryMock;
        Mock<IRepository<DeckGroup>> deckGroupRepositoryMock;
        Mock<IRepository<UserGroup>> userGroupRepositoryMock;
        GroupLogic groupLogic;

        [TestInitialize]
        public void SetUp()
        {
            userExample = new User()
            {
                Id = 1,
                Username = "Ana",
                Email = "ana@gmail.com",
                Password = "ana1234",
                IsStudent = true,
                Token = "token",
                Groups = new List<Group>(),
            };

            userTeacherExample = new User()
            {
                Id = 1,
                Username = "Mario",
                Email = "mario@gmail.com",
                Password = "mario1234_",
                IsStudent = false,
                Token = "tokenMario",
                Groups = new List<Group>(),
                Decks = new List<Deck>()
            };

            groupExample = new Group()
            {
                Id = 1,
                Name = "Clase 7",
                Creator = userExample,
                UserGroups = new List<UserGroup>(),
                DeckGroups = new List<DeckGroup>()
            };

            userGroupExample = new UserGroup()
            {
                Group = groupExample,
                GroupId = groupExample.Id,
                User = userExample,
                UserId = userExample.Id
            };

            deckExample = new Deck()
            {
                Id = 1,
                Author = userTeacherExample,
                DeckGroups = new List<DeckGroup>(),
                Difficulty = Difficulty.Easy,
                Flashcards = new List<Flashcard>(),
                IsHidden = false,
                Name = "DeckName",
                Subject = "English"
            };

            groupRepositoryMock = new Mock<IRepository<Group>>(MockBehavior.Strict);
            userRepositoryMock = new Mock<IRepository<User>>(MockBehavior.Strict);
            userTokenRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            userGroupRepositoryMock = new Mock<IRepository<UserGroup>>(MockBehavior.Strict);
            deckGroupRepositoryMock = new Mock<IRepository<DeckGroup>>(MockBehavior.Strict);
            deckRepositoryMock = new Mock<IRepository<Deck>>(MockBehavior.Strict);
            groupLogic = new GroupLogic(groupRepositoryMock.Object, userTokenRepositoryMock.Object,
                userRepositoryMock.Object, userGroupRepositoryMock.Object, deckRepositoryMock.Object,
                deckGroupRepositoryMock.Object);
        }

        [TestMethod]
        public void AddDeckOkTest()
        {
            groupRepositoryMock.Setup(m => m.Add(It.IsAny<Group>()));
            userTokenRepositoryMock.Setup(m => m.GetUserByToken(It.IsAny<string>())).Returns(userExample);
            userRepositoryMock.Setup(a => a.Update(It.IsAny<User>()));

            var result = groupLogic.AddGroup(groupExample, "token");

            groupRepositoryMock.VerifyAll();
            Assert.AreEqual(groupExample, result);
        }

        [TestMethod]
        [ExpectedException(typeof(AlreadyExistsException))]
        public void AddDeckRepeatedNameTest()
        {
            userExample.Groups.Add(groupExample);
            groupRepositoryMock.Setup(m => m.Add(It.IsAny<Group>()));
            userTokenRepositoryMock.Setup(m => m.GetUserByToken(It.IsAny<string>())).Returns(userExample);
            userRepositoryMock.Setup(a => a.Update(It.IsAny<User>()));

            var result = groupLogic.AddGroup(groupExample, "token");

            groupRepositoryMock.VerifyAll();
            Assert.AreEqual(groupExample, result);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidException))]
        public void AddDeckEmptyNameTest()
        {
            groupExample.Name = "   ";
            groupRepositoryMock.Setup(m => m.Add(It.IsAny<Group>()));
            userTokenRepositoryMock.Setup(m => m.GetUserByToken(It.IsAny<string>())).Returns(userExample);
            userRepositoryMock.Setup(a => a.Update(It.IsAny<User>()));

            var result = groupLogic.AddGroup(groupExample, "token");

            groupRepositoryMock.VerifyAll();
            Assert.AreEqual(groupExample, result);
        }

        [TestMethod]
        public void SubscribeOkTest()
        {
            userTokenRepositoryMock.Setup(m => m.GetUserByToken(It.IsAny<string>())).Returns(userExample);
            groupRepositoryMock.Setup(m => m.GetById(It.IsAny<int>())).Returns(groupExample);
            userGroupRepositoryMock.Setup(a => a.FindByCondition(It.IsAny<Expression<Func<UserGroup,
                bool>>>())).Returns(new List<UserGroup>() { });
            groupRepositoryMock.Setup(a => a.Update(It.IsAny<Group>()));

            var result = groupLogic.Subscribe(userExample.Token, 1);
            groupRepositoryMock.VerifyAll();
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void UnsubscribeOkTest()
        {
            userTokenRepositoryMock.Setup(m => m.GetUserByToken(It.IsAny<string>())).Returns(userExample);
            groupRepositoryMock.Setup(m => m.GetById(It.IsAny<int>())).Returns(groupExample);
            userGroupRepositoryMock.Setup(a => a.FindByCondition(It.IsAny<Expression<Func<UserGroup,
                bool>>>())).Returns(new List<UserGroup>() { userGroupExample });
            userGroupRepositoryMock.Setup(a => a.Delete(It.IsAny<UserGroup>()));
            groupRepositoryMock.Setup(b => b.Update(It.IsAny<Group>()));

            var result = groupLogic.Unsubscribe(userExample.Token, 1);
            groupRepositoryMock.VerifyAll();
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void UserIsSubscribedTest()
        {
            userTokenRepositoryMock.Setup(m => m.GetUserByToken(It.IsAny<string>())).Returns(userExample);
            userGroupRepositoryMock.Setup(a => a.FindByCondition(It.IsAny<Expression<Func<UserGroup,
                bool>>>())).Returns(new List<UserGroup>() { userGroupExample });

            var result = groupLogic.UserIsSubscribed(userExample.Token, 1);
            groupRepositoryMock.VerifyAll();
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void UserIsNotSubscribedTest()
        {
            userTokenRepositoryMock.Setup(m => m.GetUserByToken(It.IsAny<string>())).Returns(userExample);
            userGroupRepositoryMock.Setup(a => a.FindByCondition(It.IsAny<Expression<Func<UserGroup,
                bool>>>())).Returns(new List<UserGroup>() { });

            var result = groupLogic.UserIsSubscribed(userExample.Token, 1);
            groupRepositoryMock.VerifyAll();
            Assert.IsFalse(result);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidException))]
        public void UserIsSubscribedUnauthenticatedTest()
        {
            userTokenRepositoryMock.Setup(m => m.GetUserByToken(It.IsAny<string>())).Throws(new InvalidException(It.IsAny<string>()));

            var result = groupLogic.UserIsSubscribed("wrongtoken", 1);
            groupRepositoryMock.VerifyAll();
        }

        [TestMethod]
        public void GetAllGroupsTest()
        {
            Group group = new Group()
            {
                Creator = userExample,
                Id = 2,
                Name = "Grupo",
                UserGroups = new List<UserGroup>()
            };

            groupRepositoryMock.Setup(b => b.FindByCondition(It.IsAny<Expression<Func<Group,
                bool>>>())).Returns(new List<Group>() { group });

            var result = groupLogic.GetAllGroups(group.Name).Count();

            groupRepositoryMock.VerifyAll();

            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void AssignOkTest()
        {
            userTokenRepositoryMock.Setup(m => m.GetUserByToken(It.IsAny<string>())).Returns(userExample);
            groupRepositoryMock.Setup(m => m.GetById(It.IsAny<int>())).Returns(groupExample);
            deckRepositoryMock.Setup(m => m.GetById(It.IsAny<int>())).Returns(deckExample);
            groupRepositoryMock.Setup(a => a.Update(It.IsAny<Group>()));
            deckGroupRepositoryMock.Setup(a => a.FindByCondition(It.IsAny<Expression<Func<DeckGroup,
               bool>>>())).Returns(new List<DeckGroup>() { });

            var result = groupLogic.Assign(userExample.Token, 1, 1);
            groupRepositoryMock.VerifyAll();
            Assert.AreEqual(groupExample, result);
        }

        [TestMethod]
        public void UnassignOkTest()
        {
            userTokenRepositoryMock.Setup(m => m.GetUserByToken(It.IsAny<string>())).Returns(userExample);
            groupRepositoryMock.Setup(m => m.GetById(It.IsAny<int>())).Returns(groupExample);
            deckGroupRepositoryMock.Setup(a => a.FindByCondition(It.IsAny<Expression<Func<DeckGroup,
                bool>>>())).Returns(new List<DeckGroup>() { It.IsAny<DeckGroup>() });
            deckGroupRepositoryMock.Setup(a => a.Delete(It.IsAny<DeckGroup>()));
            groupRepositoryMock.Setup(b => b.Update(It.IsAny<Group>()));

            var result = groupLogic.Unassign(userExample.Token, 1, 1);
            groupRepositoryMock.VerifyAll();
            Assert.AreEqual(result, groupExample);
        }
    }
}
