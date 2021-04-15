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
        Mock<IUserRepository> userTokenRepository;
        DeckLogic deckLogic;

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
                Token = "token"
            };

            deckExample = new Deck()
            {
                Id = 1,
                Name = "Clase 7",
                Author = userExample,
                Difficulty = Domain.Enumerations.Difficulty.Medium,
                IsHidden = false,
                Subject = "English",
                Flashcards = new List<Flashcard>()
            };

            deckRepositoryMock = new Mock<IRepository<Deck>>(MockBehavior.Strict);
            userRepositoryMock = new Mock<IRepository<User>>(MockBehavior.Strict);
            userTokenRepository = new Mock<IUserRepository>(MockBehavior.Strict);
            deckLogic = new DeckLogic(deckRepositoryMock.Object, userRepositoryMock.Object,
                 userTokenRepository.Object);
        }

        [TestMethod]
        public void AddDeckOkTest()
        {
            deckRepositoryMock.Setup(m => m.Add(It.IsAny<Deck>()));
            deckRepositoryMock.Setup(m => m.GetAll()).Returns(new List<Deck>());
            userRepositoryMock.Setup(m => m.GetById(1)).Returns(userExample);
            userTokenRepository.Setup(m => m.GetUserByToken(It.IsAny<string>())).Returns(userExample);
            userRepositoryMock.Setup(a => a.Update(It.IsAny<User>()));

            userRepositoryMock.Setup(m => m.Add(It.IsAny<User>()));

            var result = deckLogic.AddDeck(deckExample, userExample.Token);

            deckRepositoryMock.VerifyAll();
            Assert.AreEqual(deckExample, result);
        }

        [ExpectedException(typeof(InvalidException))]
        [TestMethod]
        public void AddDeckNullNameTest()
        {
            deckExample.Name = null;
            deckRepositoryMock.Setup(m => m.Add(It.IsAny<Deck>()));
            deckRepositoryMock.Setup(m => m.GetAll()).Returns(new List<Deck>());
            userRepositoryMock.Setup(m => m.GetById(1)).Returns(userExample);
            userTokenRepository.Setup(m => m.GetUserByToken(It.IsAny<string>())).Returns(userExample);
            userRepositoryMock.Setup(a => a.Update(It.IsAny<User>()));

            userRepositoryMock.Setup(m => m.Add(It.IsAny<User>()));

            var result = deckLogic.AddDeck(deckExample, userExample.Token);

            deckRepositoryMock.VerifyAll();
        }

        [ExpectedException(typeof(AlreadyExistsException))]
        [TestMethod]
        public void AddDeckNameAlreadyExistsTest()
        {
            Deck anotherDeckExample = new Deck()
            {
                Id = 2,
                Name = "Clase 7",
                Author = userExample,
                Difficulty = Domain.Enumerations.Difficulty.Hard,
                IsHidden = true,
                Subject = "Russian",
                Flashcards = new List<Flashcard>()
            };

            deckRepositoryMock.Setup(m => m.Add(It.IsAny<Deck>()));
            deckRepositoryMock.Setup(m => m.GetAll()).Returns(new List<Deck>() { anotherDeckExample });
            userRepositoryMock.Setup(m => m.GetById(1)).Returns(userExample);
            userTokenRepository.Setup(m => m.GetUserByToken(It.IsAny<string>())).Returns(userExample);
            userRepositoryMock.Setup(a => a.Update(It.IsAny<User>()));

            userRepositoryMock.Setup(m => m.Add(It.IsAny<User>()));

            deckLogic.AddDeck(anotherDeckExample, userExample.Token);
            var result = deckLogic.AddDeck(deckExample, userExample.Token);

            deckRepositoryMock.VerifyAll();
        }

        [ExpectedException(typeof(InvalidException))]
        [TestMethod]
        public void AddDeckNullSubjectTest()
        {
            deckExample.Subject = null;
            deckRepositoryMock.Setup(m => m.Add(It.IsAny<Deck>()));
            deckRepositoryMock.Setup(m => m.GetAll()).Returns(new List<Deck>());
            userRepositoryMock.Setup(m => m.GetById(1)).Returns(userExample);
            userTokenRepository.Setup(m => m.GetUserByToken(It.IsAny<string>())).Returns(userExample);
            userRepositoryMock.Setup(a => a.Update(It.IsAny<User>()));

            userRepositoryMock.Setup(m => m.Add(It.IsAny<User>()));

            var result = deckLogic.AddDeck(deckExample, userExample.Token);

            deckRepositoryMock.VerifyAll();
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

        [TestMethod]
        public void EditDeckOkTest()
        {
            string newName = "new name";
            Difficulty newDifficulty = Difficulty.Hard;
            bool newVisibility = true;

            deckRepositoryMock.Setup(m => m.Update(It.IsAny<Deck>()));
            deckRepositoryMock.Setup(m => m.GetById(1)).Returns(deckExample);
            deckRepositoryMock.Setup(m => m.FindByCondition(a => a.Name == newName && a.Id != 1)).Returns(new List<Deck>() { });

            deckExample.Name = newName;
            deckExample.IsHidden = newVisibility;
            deckExample.Difficulty = newDifficulty;
            var result = deckLogic.EditDeck(1, newName, newDifficulty, newVisibility);

            deckRepositoryMock.VerifyAll();

            Assert.IsTrue(result.Equals(deckExample));
        }

        [ExpectedException(typeof(AlreadyExistsException))]
        [TestMethod]
        public void EditDeckNameAlreadyExistsTest()
        {
            Deck anotherDeckExample = new Deck()
            {
                Author = userExample,
                Difficulty = Difficulty.Easy,
                Flashcards = new List<Flashcard>(),
                Id = 2,
                IsHidden = false,
                Name = "another deck",
                Subject = "P.E"
            };

            Difficulty newDifficulty = Difficulty.Hard;
            bool newVisibility = true;

            deckRepositoryMock.Setup(m => m.Update(It.IsAny<Deck>()));
            deckRepositoryMock.Setup(m => m.GetById(1)).Returns(deckExample);
            deckRepositoryMock.Setup(m => m.Add(deckExample));
            deckRepositoryMock.Setup(m => m.Add(anotherDeckExample));

            string newName = anotherDeckExample.Name;
            int deckId = deckExample.Id;
            deckRepositoryMock.Setup(m => m.FindByCondition(a => a.Name == newName && a.Id != deckId)).Returns(new List<Deck>() { anotherDeckExample });

            var result = deckLogic.EditDeck(1, anotherDeckExample.Name, newDifficulty, newVisibility);

            deckRepositoryMock.VerifyAll();
        }

        [TestMethod]
        public void GetDeckByIdTest()
        {
            deckRepositoryMock.Setup(b => b.GetById(1)).Returns(deckExample);

            var result = deckLogic.GetDeckById(1);

            deckRepositoryMock.VerifyAll();

            Assert.AreEqual(deckExample, result);
        }

        [ExpectedException(typeof(NotFoundException))]
        [TestMethod]
        public void GetDeckByIdNotFoundTest()
        {
            deckRepositoryMock.Setup(b => b.GetById(7)).Throws(new NotFoundException(It.IsAny<string>()));
            var result = deckLogic.GetDeckById(7);
        }
    }
}
