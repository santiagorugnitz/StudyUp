using BusinessLogic;
using DataAccessInterface;
using Domain;
using Domain.Enumerations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLogicTest
{
    [TestClass]
    public class GroupLogicTest
    {
        User userExample;
        Group groupExample;
        Mock<IRepository<Group>> groupRepositoryMock;
        Mock<IUserRepository> userTokenRepositoryMock;
        Mock<IRepository<User>> userRepositoryMock;
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
                Groups = new List<Group>()
            };

            groupExample = new Group()
            {
                Id = 1,
                Name = "Clase 7",
                Creator = userExample
            };

            groupRepositoryMock = new Mock<IRepository<Group>>(MockBehavior.Strict);
            userRepositoryMock = new Mock<IRepository<User>>(MockBehavior.Strict);
            userTokenRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            groupLogic = new GroupLogic(groupRepositoryMock.Object, userTokenRepositoryMock.Object, userRepositoryMock.Object);
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
    }
}
