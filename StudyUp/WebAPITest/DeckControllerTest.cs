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
                Username = "Jose",
                IsStudent = false,
                Email = "jose@hotmail.com",
                Password = "contraseña123",
                Token = "token"
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
            logicMock.Setup(x => x.AddDeck(It.IsAny<Deck>(), It.IsAny<string>())).Returns(new Deck());

            var result = controller.Post(deckModelExample, userModelExample.Token);
            var okResult = result as OkObjectResult;
            var value = okResult.Value as User;

            logicMock.VerifyAll();
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidException))]
        public void PostDeckBadRequestTest()
        {
            DeckModel anotherDeckModelExample = new DeckModel()
            {
                Name = "Clase 7",
                Author = userModelExample.ToEntity(),
                Difficulty = Domain.Enumerations.Difficulty.Hard,
                IsHidden = false,
                Subject = "Latin"
            };
            logicMock.Setup(x => x.AddDeck(anotherDeckModelExample.ToEntity(), userModelExample.Token)).Throws(new InvalidException(DeckMessage.EMPTY_NAME_MESSAGE));

            var result = controller.Post(anotherDeckModelExample, userModelExample.Token);
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
            logicMock.Setup(x => x.AddDeck(It.IsAny<Deck>(), userModelExample.Token)).Returns(new Deck());
            logicMock.Setup(x => x.EditDeck(1, "new name", Domain.Enumerations.Difficulty.Easy,
                true)).Returns(new Deck());

            UpdateDeckModel updateDeckModel = new UpdateDeckModel()
            {
                Difficulty = Domain.Enumerations.Difficulty.Easy,
                Name = "new name",
                IsHidden = true
            };
            controller.Post(deckModelExample, userModelExample.Token);

            var result = controller.Update(1, updateDeckModel);
            var okResult = result as OkObjectResult;
            var value = okResult.Value as Deck;

            logicMock.VerifyAll();
        }

        [TestMethod]
        public void GetDeckByIdOkTest()
        {
            logicMock.Setup(x => x.GetDeckById(1)).Returns(It.IsAny<Deck>());

            var result = controller.GetDeckById(1);
            var okResult = result as OkObjectResult;
            var value = okResult.Value as List<Deck>;

            logicMock.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public void GetDeckByIdBadRequestTest()
        {
            logicMock.Setup(x => x.GetDeckById(1)).Throws(new NotFoundException(DeckMessage.DECK_NOT_FOUND));

            var result = controller.GetDeckById(1);
            var okResult = result as BadRequestObjectResult;
        }

        [TestMethod]
        public void DeleteDeckOkTest()
        {
            logicMock.Setup(x => x.DeleteDeck(1, It.IsAny<string>())).Returns(true);

            var result = controller.Delete(1, "token");
            var okResult = result as OkObjectResult;
            var value = okResult.Value as bool?;
            logicMock.VerifyAll();
        }
    }
}
