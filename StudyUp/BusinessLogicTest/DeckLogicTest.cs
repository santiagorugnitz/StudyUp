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
        Mock<IRepository<User>> userRepositoryMock;
        DeckLogic deckLogic;

        [TestInitialize]
        public void SetUp()
        {
            userExample = new User()
            {
                Id = 1,
                Name = "Ana",
                Email = "ana@gmail.com",
                Password = "ana1234",
                IsATeacher = true
            };

            deckExample = new Deck()
            {
                Id =1,
                Name = "Clase 7",
                Author = userExample,
                Difficulty = Domain.Enumerations.Difficulty.Medium,
                IsHidden = false,
                Subject = "English",
                Flashcards = new List<Flashcard>()
            };

            deckRepositoryMock = new Mock<IRepository<Deck>>(MockBehavior.Strict);
            userRepositoryMock = new Mock<IRepository<User>>(MockBehavior.Strict);
            deckLogic = new DeckLogic(deckRepositoryMock.Object, userRepositoryMock.Object);
        }

        [TestMethod]
        public void AddDeckOkTest()
        {
            deckRepositoryMock.Setup(m => m.Add(It.IsAny<Deck>()));
            deckRepositoryMock.Setup(m => m.GetAll()).Returns(new List<Deck>());
            userRepositoryMock.Setup(m => m.GetById(1)).Returns(userExample);
            userRepositoryMock.Setup(a => a.Update(It.IsAny<User>()));

            userRepositoryMock.Setup(m => m.Add(It.IsAny<User>()));

            var result = deckLogic.AddDeck(deckExample, userExample.Id);

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

        [TestMethod]
        public void GetDecksByAuthorTest()
        {
            deckRepositoryMock.Setup(b => b.FindByCondition(d => d.Author.Id == 1)).Returns(new List<Deck>() { deckExample });
            
            var result = deckLogic.GetDecksByAuthor(1).Count();

            deckRepositoryMock.VerifyAll();

            Assert.AreEqual(1, result);
        }
    }
}
