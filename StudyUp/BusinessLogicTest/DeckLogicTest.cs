using BusinessLogic;
using BusinessLogicInterface;
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
    public class DeckLogicTest
    {
        User userExample;
        Deck deckExample;
        Group groupExample;
        Mock<IRepository<Deck>> deckRepositoryMock;
        Mock<IRepository<User>> userRepositoryMock;
        Mock<IRepository<Group>> groupRepositoryMock;
        Mock<IRepository<DeckGroup>> deckGroupRepositoryMock;
        Mock<IRepository<Flashcard>> flashcardRepositoryMock;
        Mock<IUserRepository> userTokenRepository;
        DeckLogic deckLogic;
        Mock<INotifications> notificationsInterfaceMock;

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
                Decks = new List<Deck>()
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

            groupExample = new Group()
            {
                Id = 1,
                Name = "GroupExample",
                Creator = userExample,
                UserGroups = new List<UserGroup>(),
                DeckGroups = new List<DeckGroup>()
            };

            deckRepositoryMock = new Mock<IRepository<Deck>>(MockBehavior.Strict);
            userRepositoryMock = new Mock<IRepository<User>>(MockBehavior.Loose);
            flashcardRepositoryMock = new Mock<IRepository<Flashcard>>(MockBehavior.Strict);
            userTokenRepository = new Mock<IUserRepository>(MockBehavior.Strict);
            groupRepositoryMock = new Mock<IRepository<Group>>(MockBehavior.Strict);
            deckGroupRepositoryMock = new Mock<IRepository<DeckGroup>>(MockBehavior.Strict);
            notificationsInterfaceMock = new Mock<INotifications>(MockBehavior.Strict);
            deckLogic = new DeckLogic(deckRepositoryMock.Object, userRepositoryMock.Object,
                 userTokenRepository.Object, flashcardRepositoryMock.Object,
                 deckGroupRepositoryMock.Object, groupRepositoryMock.Object,
                 notificationsInterfaceMock.Object);
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

        [ExpectedException(typeof(NotAuthenticatedException))]
        [TestMethod]
        public void AddDeckBadToken()
        {
            deckRepositoryMock.Setup(m => m.Add(It.IsAny<Deck>()));
            deckRepositoryMock.Setup(m => m.GetAll()).Returns(new List<Deck>());
            userRepositoryMock.Setup(m => m.GetById(1)).Returns(userExample);
            userTokenRepository.Setup(m => m.GetUserByToken(It.IsAny<string>())).Returns((User)null);
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
            userRepositoryMock.Setup(m => m.GetById(1)).Returns(userExample);
            deckRepositoryMock.Setup(b => b.FindByCondition(d => d.Author.Id == 1)).Returns(new List<Deck>() { deckExample });

            var result = deckLogic.GetDecksByAuthor(1).Count();

            deckRepositoryMock.VerifyAll();

            Assert.AreEqual(1, result);
        }

        [ExpectedException(typeof(NotFoundException))]
        [TestMethod]
        public void GetDecksByIncorrectAuthorTest()
        {
            userRepositoryMock.Setup(m => m.GetById(1)).Returns((User)null);
            deckRepositoryMock.Setup(b => b.FindByCondition(d => d.Author.Id == 1)).Returns(new List<Deck>() { deckExample });

            var result = deckLogic.GetDecksByAuthor(1).Count();

            deckRepositoryMock.VerifyAll();

            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void EditDeckOkTest()
        {
            string newSubject = "new subject";
            string newName = "new name";
            Difficulty newDifficulty = Difficulty.Hard;
            bool newVisibility = true;

            deckRepositoryMock.Setup(m => m.Update(It.IsAny<Deck>()));
            deckRepositoryMock.Setup(m => m.GetById(1)).Returns(deckExample);
            deckRepositoryMock.Setup(m => m.FindByCondition(a => a.Name == newName && a.Id != 1)).Returns(new List<Deck>() { });

            deckExample.Name = newName;
            deckExample.IsHidden = newVisibility;
            deckExample.Difficulty = newDifficulty;
            deckExample.Subject = newSubject;
            var result = deckLogic.EditDeck(1, newName, newDifficulty, newVisibility, newSubject);

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

            string newSubject = anotherDeckExample.Subject;
            string newName = anotherDeckExample.Name;
            int deckId = deckExample.Id;
            deckRepositoryMock.Setup(m => m.FindByCondition(a => a.Name == newName && a.Id != deckId)).Returns(new List<Deck>() { anotherDeckExample });

            var result = deckLogic.EditDeck(1, anotherDeckExample.Name, newDifficulty, newVisibility, newSubject);

            deckRepositoryMock.VerifyAll();
        }

        [ExpectedException(typeof(InvalidException))]
        [TestMethod]
        public void EditDeckEmptySubjectTest()
        {
            Difficulty newDifficulty = Difficulty.Hard;
            bool newVisibility = true;

            deckRepositoryMock.Setup(m => m.Update(It.IsAny<Deck>()));
            deckRepositoryMock.Setup(m => m.GetById(1)).Returns(deckExample);
            deckRepositoryMock.Setup(m => m.Add(deckExample));

            deckRepositoryMock.Setup(m => m.FindByCondition(a => a.Name == "Name" && a.Id != 1)).Returns(new List<Deck>());

            var result = deckLogic.EditDeck(1, "Name", newDifficulty, newVisibility, "");

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

        [ExpectedException(typeof(NotFoundException))]
        [TestMethod]
        public void DeleteDeckNotFoundTest()
        {
            deckRepositoryMock.Setup(b => b.GetById(1)).Throws(new NotFoundException(It.IsAny<string>()));
            deckRepositoryMock.Setup(d => d.Delete(deckExample));

            var result = deckLogic.DeleteDeck(1, "token");
        }

        [ExpectedException(typeof(InvalidException))]
        [TestMethod]
        public void DeleteDeckDifferentAuthorTest()
        {
            deckRepositoryMock.Setup(b => b.GetById(deckExample.Id)).Throws(new InvalidException(It.IsAny<string>()));
            deckRepositoryMock.Setup(d => d.Delete(deckExample));
            var result = deckLogic.DeleteDeck(deckExample.Id, "different token");
        }

        [TestMethod]
        public void DeleteDeckOkTest()
        {
            Flashcard flashcard = new Flashcard()
            {
                Id = 1,
                Answer = "Choose questions wisely.",
                Question = "How do you write a good answer?",
            };
            deckExample.Flashcards.Add(flashcard);

            deckRepositoryMock.Setup(b => b.GetById(1)).Returns(deckExample);
            deckRepositoryMock.Setup(b => b.Delete(deckExample));
            flashcardRepositoryMock.Setup(b => b.Delete(flashcard));


            var result = deckLogic.DeleteDeck(1, "token");

            deckRepositoryMock.VerifyAll();

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void AssignOkTest()
        {
            userTokenRepository.Setup(m => m.GetUserByToken(It.IsAny<string>())).Returns(userExample);
            groupRepositoryMock.Setup(m => m.GetById(It.IsAny<int>())).Returns(groupExample);
            deckRepositoryMock.Setup(m => m.GetById(It.IsAny<int>())).Returns(deckExample);
            groupRepositoryMock.Setup(a => a.Update(It.IsAny<Group>()));
            deckGroupRepositoryMock.Setup(a => a.FindByCondition(It.IsAny<Expression<Func<DeckGroup,
               bool>>>())).Returns(new List<DeckGroup>() { });
            notificationsInterfaceMock.Setup(a => a.NotifyMaterial(It.IsAny<int>(), It.IsAny<Group>()));

            var result = deckLogic.Assign(userExample.Token, 1, 1);
            deckRepositoryMock.VerifyAll();
            Assert.AreEqual(groupExample, result);
        }

        [TestMethod]
        public void UnassignOkTest()
        {
            userTokenRepository.Setup(m => m.GetUserByToken(It.IsAny<string>())).Returns(userExample);
            groupRepositoryMock.Setup(m => m.GetById(It.IsAny<int>())).Returns(groupExample);
            deckGroupRepositoryMock.Setup(a => a.FindByCondition(It.IsAny<Expression<Func<DeckGroup,
                bool>>>())).Returns(new List<DeckGroup>() { It.IsAny<DeckGroup>() });
            deckGroupRepositoryMock.Setup(a => a.Delete(It.IsAny<DeckGroup>()));
            groupRepositoryMock.Setup(b => b.Update(It.IsAny<Group>()));

            var result = deckLogic.Unassign(userExample.Token, 1, 1);
            deckRepositoryMock.VerifyAll();
            Assert.AreEqual(result, groupExample);
        }
    }
}
