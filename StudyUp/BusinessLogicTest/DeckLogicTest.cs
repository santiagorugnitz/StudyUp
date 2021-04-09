using BusinessLogic;
using DataAccessInterface;
using Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
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

    }
}
