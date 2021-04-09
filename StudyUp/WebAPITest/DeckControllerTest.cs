using BusinessLogicInterface;
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
    public class DeckControllerTest
    {
        Mock<IDeckLogic> logicMock;
        DeckController controller;
        DeckModel deckModelExample;
        UserModel userModelExample;

        [TestInitialize]
        public void SetUp()
        {
            logicMock = new Mock<IDeckLogic>(MockBehavior.Strict);
            controller = new DeckController(logicMock.Object);


            userModelExample = new UserModel()
            {
                Name = "Jose",
                IsATeacher = false,
                Email = "jose@hotmail.com",
                Password = "contraseña123",
            };

            deckModelExample = new DeckModel()
            {
                Name = "Clase 7",
                Author = userModelExample.ToEntity(),
                Difficulty = Domain.Enumerations.Difficulty.Easy,
                IsHidden = false,
                Subject = "French"
            };
        }

        [TestMethod]
        public void PostDeckOkTest()
        {
            logicMock.Setup(x => x.AddDeck(It.IsAny<Deck>())).Returns(new Deck());

            var result = controller.Post(deckModelExample);
            var okResult = result as OkObjectResult;
            var value = okResult.Value as User;

            logicMock.VerifyAll();
        }

        [TestMethod]
        public void GetDecksOkTest()
        {
            logicMock.Setup(x => x.GetAllDecks()).Returns(new List<Deck>());

            var result = controller.GetAllDecks();
            var okResult = result as OkObjectResult;
            var value = okResult.Value as List<Deck>;

            logicMock.VerifyAll();
        }
    }
}
