using BusinessLogic;
using DataAccessInterface;
using Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLogicTest
{
    [TestClass]
    public class DeckLogicTest
    {
        User userExample;
        Deck deckExample;
        Mock<IRepository<Deck>> deckRepositoryMock;
        DeckLogic deckLogic;

        [TestInitialize]
        public void SetUp()
        {
            userExample = new User()
            {
                Name = "Ana",
                Email = "ana@gmail.com",
                Password = "ana1234",
                IsATeacher = true
            };

            deckExample = new Deck()
            {
                Name = "Clase 7",
                Author = userExample,
                Difficulty = Domain.Enumerations.Difficulty.Medium,
                IsHidden = false,
                Subject = "English",
                Flashcards = new List<Flashcard>()
            };

            deckRepositoryMock = new Mock<IRepository<Deck>>(MockBehavior.Strict);
            deckLogic = new DeckLogic(deckRepositoryMock.Object);
        }

        [TestMethod]
        public void AddDeckOkTest()
        {
            deckRepositoryMock.Setup(m => m.Add(It.IsAny<Deck>()));
            deckRepositoryMock.Setup(m => m.GetAll()).Returns(new List<Deck>());

            var result = deckLogic.AddDeck(deckExample);

            deckRepositoryMock.VerifyAll();
            Assert.AreEqual(deckExample, result);
        }

        [TestMethod]
        public void GetAllDecksTest()
        {
            Deck deck = new Deck
            {
                Id = 1,
                Name = "Clase 02",
                Author = userExample,
                Difficulty = Domain.Enumerations.Difficulty.Hard,
                IsHidden = false,
                Flashcards = new List<Flashcard>(),
                Subject = "German" 
            };

            deckRepositoryMock.Setup(b => b.GetAll()).Returns(new List<Deck>() { deck });

            var result = deckLogic.GetAllDecks().Count();

            deckRepositoryMock.VerifyAll();

            Assert.AreEqual(1, result);
        }
    }
}
