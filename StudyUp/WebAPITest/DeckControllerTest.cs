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
        Deck deck;

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
                Difficulty = Domain.Enumerations.Difficulty.Easy,
                IsHidden = false,
                Subject = "French"
            };
            deck = new Deck
            {
                Id = 1,
                Author = new User { Username = "" },
                Difficulty = Domain.Enumerations.Difficulty.Easy,
                Name = "",
                Subject = "",
                IsHidden = true,
            };
        }

        [TestMethod]
        public void PostDeckOkTest()
        {
            logicMock.Setup(x => x.AddDeck(It.IsAny<Deck>(), It.IsAny<string>())).Returns(deck);

            var result = controller.Post(deckModelExample, userModelExample.Token);
            var okResult = result as OkObjectResult;

            logicMock.VerifyAll();
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidException))]
        public void PostDeckBadRequestTest()
        {
            DeckModel anotherDeckModelExample = new DeckModel()
            {
                Name = "Clase 7",
                Difficulty = Domain.Enumerations.Difficulty.Hard,
                IsHidden = false,
                Subject = "Latin"
            };
            logicMock.Setup(x => x.AddDeck(anotherDeckModelExample.ToEntity(), userModelExample.Token)).Throws(new InvalidException(DeckMessage.EMPTY_NAME));

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

            var result = controller.GetAllDecks(1);
            var okResult = result as OkObjectResult;
            var value = okResult.Value as List<Deck>;

            logicMock.VerifyAll();
        }

        [TestMethod]
        public void UpdateDeckTest()
        {
            logicMock.Setup(x => x.AddDeck(It.IsAny<Deck>(), userModelExample.Token)).Returns(deck);
            logicMock.Setup(x => x.EditDeck(1, "new name", Domain.Enumerations.Difficulty.Easy,
                true, "new subject")).Returns(deck);

            UpdateDeckModel updateDeckModel = new UpdateDeckModel()
            {
                Difficulty = Domain.Enumerations.Difficulty.Easy,
                Name = "new name",
                IsHidden = true,
                Subject = "new subject"
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
            var deck = deckModelExample.ToEntity();
            deck.Flashcards = new List<Flashcard>();
            deck.Author = new User { Username = "test" };
            logicMock.Setup(x => x.GetDeckById(1)).Returns(deck);

            var result = controller.GetDeckById(1);
            var okResult = result as OkObjectResult;
            var value = okResult.Value as ResponseFullDeckModel;

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
            logicMock.VerifyAll();
        }

        [TestMethod]
        public void AssingOkTest()
        {
            logicMock.Setup(x => x.Assign(It.IsAny<string>(), It.IsAny<int>(),
                It.IsAny<int>())).Returns(It.IsAny<Group>());

            var result = controller.Assign("token", 1, 1);
            var okResult = result as OkObjectResult;
            var value = okResult.Value as Group;

            logicMock.VerifyAll();
        }

        [TestMethod]
        public void UnassignOkTest()
        {
            logicMock.Setup(x => x.Unassign(It.IsAny<string>(), It.IsAny<int>(),
                It.IsAny<int>())).Returns(It.IsAny<Group>());

            var result = controller.Unassign("token", 1, 1);
            var okResult = result as OkObjectResult;
            var value = okResult.Value as Group;

            logicMock.VerifyAll();
        }
    }
}
