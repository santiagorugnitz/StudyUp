using BusinessLogicInterface;
using Domain;
using Exceptions;
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
                Id = 1,
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
            logicMock.Setup(x => x.AddDeck(It.IsAny<Deck>(), 1)).Returns(new Deck());

            var result = controller.Post(deckModelExample, userModelExample.Id);
            var okResult = result as OkObjectResult;
            var value = okResult.Value as User;

            logicMock.VerifyAll();
        }

        [TestMethod]
        public void PostDeckBadRequestTest()
        {
            DeckModel anotherDeckModelExample = new DeckModel()
            {
                Name = null,
                Author = userModelExample.ToEntity(),
                Difficulty = Domain.Enumerations.Difficulty.Hard,
                IsHidden = false,
                Subject = "Latin"
            };
            logicMock.Setup(x => x.AddDeck(anotherDeckModelExample.ToEntity(), 1)).Throws(new InvalidException(DeckMessage.EMPTY_NAME_MESSAGE));

            var result = controller.Post(anotherDeckModelExample, 1);
            var okResult = result as BadRequestObjectResult;

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

        [TestMethod]
        public void GetDecksByAuthorOkTest()
        {
            logicMock.Setup(x => x.GetDecksByAuthor(1)).Returns(new List<Deck>() { deckModelExample.ToEntity() });

            var result = controller.GetDecksByAuthor(1);
            var okResult = result as OkObjectResult;
            var value = okResult.Value as List<Deck>;

            logicMock.VerifyAll();
        }

        [TestMethod]
        public void UpdateDeckTest()
        {
            logicMock.Setup(x => x.AddDeck(It.IsAny<Deck>(), userModelExample.Id)).Returns(new Deck());
            logicMock.Setup(x => x.EditDeck(1, "new name", Domain.Enumerations.Difficulty.Easy,
                true)).Returns(new Deck());

            UpdateDeckModel updateDeckModel = new UpdateDeckModel()
            {
                Difficulty = Domain.Enumerations.Difficulty.Easy,
                Name = "new name",
                IsHidden = true
            };
            controller.Post(deckModelExample, userModelExample.Id);

            var result = controller.Update(1, updateDeckModel);
            var okResult = result as OkObjectResult;
            var value = okResult.Value as Deck;

            logicMock.VerifyAll();
        }
    }
}
