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
        Mock<IRepository<FlashcardComment>> flashcardCommentRepositoryMock;

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
            flashcardCommentRepositoryMock = new Mock<IRepository<FlashcardComment>>(MockBehavior.Strict);
            userTokenRepository = new Mock<IUserRepository>(MockBehavior.Strict);
            groupRepositoryMock = new Mock<IRepository<Group>>(MockBehavior.Strict);
            deckGroupRepositoryMock = new Mock<IRepository<DeckGroup>>(MockBehavior.Strict);
            notificationsInterfaceMock = new Mock<INotifications>(MockBehavior.Strict);

            deckLogic = new DeckLogic(deckRepositoryMock.Object, userRepositoryMock.Object,
                 userTokenRepository.Object, flashcardRepositoryMock.Object,
                 deckGroupRepositoryMock.Object, groupRepositoryMock.Object,
                 notificationsInterfaceMock.Object, flashcardCommentRepositoryMock.Object);
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
            deckRepositoryMock.Setup(m => m.GetAll()).Returns(new List<Deck>());

            var result = deckLogic.AddDeck(deckExample, userExample.Token);
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

            deckRepositoryMock.Setup(m => m.GetAll()).Returns(new List<Deck>() { anotherDeckExample });

            var result = deckLogic.AddDeck(deckExample, userExample.Token);
        }

        [ExpectedException(typeof(InvalidException))]
        [TestMethod]
        public void AddDeckNullSubjectTest()
        {
            deckExample.Subject = null;
            deckRepositoryMock.Setup(m => m.GetAll()).Returns(new List<Deck>());

            var result = deckLogic.AddDeck(deckExample, userExample.Token);
        }

        [ExpectedException(typeof(InvalidException))]
        [TestMethod]
        public void AddDeckInvalidDifficultyTest()
        {
            deckExample.Difficulty = (Difficulty)3;
            deckRepositoryMock.Setup(m => m.GetAll()).Returns(new List<Deck>());

            var result = deckLogic.AddDeck(deckExample, userExample.Token);
        }

        [ExpectedException(typeof(NotAuthenticatedException))]
        [TestMethod]
        public void AddDeckBadToken()
        {
            deckRepositoryMock.Setup(m => m.GetAll()).Returns(new List<Deck>());
            userTokenRepository.Setup(m => m.GetUserByToken(It.IsAny<string>())).Returns((User)null);
            var result = deckLogic.AddDeck(deckExample, userExample.Token);
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

            var result = deckLogic.GetDecksByAuthor(1).Count();
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

            deckRepositoryMock.Setup(m => m.GetById(1)).Returns(deckExample);

            string newSubject = anotherDeckExample.Subject;
            string newName = anotherDeckExample.Name;
            int deckId = deckExample.Id;
            deckRepositoryMock.Setup(m => m.FindByCondition(a => a.Name == newName && a.Id != deckId)).Returns(new List<Deck>() { anotherDeckExample });

            var result = deckLogic.EditDeck(1, anotherDeckExample.Name, newDifficulty, newVisibility, newSubject);
        }

        [ExpectedException(typeof(InvalidException))]
        [TestMethod]
        public void EditDeckEmptySubjectTest()
        {
            Difficulty newDifficulty = Difficulty.Hard;
            bool newVisibility = true;

            deckRepositoryMock.Setup(m => m.GetById(1)).Returns(deckExample);
            deckRepositoryMock.Setup(m => m.FindByCondition(a => a.Name == "Name" && a.Id != 1)).Returns(new List<Deck>());

            var result = deckLogic.EditDeck(1, "Name", newDifficulty, newVisibility, "");
        }


        [ExpectedException(typeof(InvalidException))]
        [TestMethod]
        public void EditDeckNullSubjectTest()
        {
            Difficulty newDifficulty = Difficulty.Hard;
            bool newVisibility = true;

            deckRepositoryMock.Setup(m => m.GetById(1)).Returns(deckExample);
            deckRepositoryMock.Setup(m => m.FindByCondition(a => a.Name == "Name" && a.Id != 1)).Returns(new List<Deck>());

            var result = deckLogic.EditDeck(1, "Name", newDifficulty, newVisibility, null);
        }

        [ExpectedException(typeof(NotFoundException))]
        [TestMethod]
        public void EditDeckNotFoundTest()
        {
            bool newVisibility = true;
            Difficulty newDifficulty = Difficulty.Hard;

            deckRepositoryMock.Setup(m => m.GetById(1)).Returns((Deck)null);
            deckRepositoryMock.Setup(m => m.FindByCondition(a => a.Name == "Name" && a.Id != 1)).Returns(new List<Deck>());

            var result = deckLogic.EditDeck(1, "Name", newDifficulty, newVisibility, "Subject");
        }

        [ExpectedException(typeof(InvalidException))]
        [TestMethod]
        public void EditDeckWrongDifficultyTest()
        {
            bool newVisibility = true;

            deckRepositoryMock.Setup(m => m.GetById(1)).Returns(deckExample);
            deckRepositoryMock.Setup(m => m.FindByCondition(a => a.Name == "Name" && a.Id != 1)).Returns(new List<Deck>());

            var result = deckLogic.EditDeck(1, "Name", (Difficulty)3, newVisibility, "Subject");
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
            deckRepositoryMock.Setup(b => b.GetById(7)).Returns((Deck)null);
            var result = deckLogic.GetDeckById(7);
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

            deckRepositoryMock.Setup(b => b.GetById(1)).Returns(deckExample);
            deckRepositoryMock.Setup(b => b.Delete(deckExample));

            var result = deckLogic.DeleteDeck(1, "token");

            deckRepositoryMock.VerifyAll();
            Assert.IsTrue(result);
        }

        [ExpectedException(typeof(NotFoundException))]
        [TestMethod]
        public void DeleteDeckNullDeckTest()
        {
            Flashcard flashcard = new Flashcard()
            {
                Id = 1,
                Answer = "Choose questions wisely.",
                Question = "How do you write a good answer?",
            };

            deckRepositoryMock.Setup(b => b.GetById(1)).Returns((Deck)null);

            var result = deckLogic.DeleteDeck(1, "token");
        }

        [ExpectedException(typeof(InvalidException))]
        [TestMethod]
        public void DeleteDeckInvalidTokenTest()
        {
            Flashcard flashcard = new Flashcard()
            {
                Id = 1,
                Answer = "Choose questions wisely.",
                Question = "How do you write a good answer?",
            };

            deckExample.Author.Token = "new";

            deckRepositoryMock.Setup(b => b.GetById(1)).Returns(deckExample);
            deckRepositoryMock.Setup(b => b.Delete(deckExample));

            var result = deckLogic.DeleteDeck(1, "token");
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
            notificationsInterfaceMock.Setup(a => a.NotifyMaterial(It.IsAny<Deck>(), It.IsAny<Group>()));

            var result = deckLogic.Assign(userExample.Token, 1, 1);
            deckRepositoryMock.VerifyAll();
            Assert.AreEqual(groupExample, result);
        }

        [ExpectedException(typeof(InvalidException))]
        [TestMethod]
        public void AssignNullUserTest()
        {
            userTokenRepository.Setup(m => m.GetUserByToken(It.IsAny<string>())).Returns((User)null);
            groupRepositoryMock.Setup(m => m.GetById(It.IsAny<int>())).Returns(groupExample);
            deckRepositoryMock.Setup(m => m.GetById(It.IsAny<int>())).Returns(deckExample);

            var result = deckLogic.Assign(userExample.Token, 1, 1);
        }

        [ExpectedException(typeof(InvalidException))]
        [TestMethod]
        public void AssignDistinctCreatorUserTest()
        {
            groupExample.Creator = new User() { Email = "new" };
            userTokenRepository.Setup(m => m.GetUserByToken(It.IsAny<string>())).Returns(userExample);
            groupRepositoryMock.Setup(m => m.GetById(It.IsAny<int>())).Returns(groupExample);
            deckRepositoryMock.Setup(m => m.GetById(It.IsAny<int>())).Returns(deckExample);

            var result = deckLogic.Assign(userExample.Token, 1, 1);
        }

        [ExpectedException(typeof(NotFoundException))]
        [TestMethod]
        public void AssignNullGroupTest()
        {
            userTokenRepository.Setup(m => m.GetUserByToken(It.IsAny<string>())).Returns(userExample);
            groupRepositoryMock.Setup(m => m.GetById(It.IsAny<int>())).Returns((Group)null);
            deckRepositoryMock.Setup(m => m.GetById(It.IsAny<int>())).Returns(deckExample);

            var result = deckLogic.Assign(userExample.Token, 1, 1);
        }

        [ExpectedException(typeof(NotFoundException))]
        [TestMethod]
        public void AssignNullDeckTest()
        {
            userTokenRepository.Setup(m => m.GetUserByToken(It.IsAny<string>())).Returns(userExample);
            groupRepositoryMock.Setup(m => m.GetById(It.IsAny<int>())).Returns(groupExample);
            deckRepositoryMock.Setup(m => m.GetById(It.IsAny<int>())).Returns((Deck)null);

            var result = deckLogic.Assign(userExample.Token, 1, 1);
        }

        [ExpectedException(typeof(AlreadyExistsException))]
        [TestMethod]
        public void AssignAlreadyExistsDeckTest()
        {
            userTokenRepository.Setup(m => m.GetUserByToken(It.IsAny<string>())).Returns(userExample);
            groupRepositoryMock.Setup(m => m.GetById(It.IsAny<int>())).Returns(groupExample);
            deckRepositoryMock.Setup(m => m.GetById(It.IsAny<int>())).Returns(deckExample);
            deckGroupRepositoryMock.Setup(a => a.FindByCondition(It.IsAny<Expression<Func<DeckGroup,
               bool>>>())).Returns(new List<DeckGroup>() { new DeckGroup() });

            var result = deckLogic.Assign(userExample.Token, 1, 1);
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

        [ExpectedException(typeof(InvalidException))]
        [TestMethod]
        public void UnassignNullUserTest()
        {
            userTokenRepository.Setup(m => m.GetUserByToken(It.IsAny<string>())).Returns((User)null);
            groupRepositoryMock.Setup(m => m.GetById(It.IsAny<int>())).Returns(groupExample);

            var result = deckLogic.Unassign(userExample.Token, 1, 1);
        }

        [ExpectedException(typeof(InvalidException))]
        [TestMethod]
        public void UnassignInvalidCreatorTest()
        {
            groupExample.Creator = new User() { Email = "new" };
            userTokenRepository.Setup(m => m.GetUserByToken(It.IsAny<string>())).Returns(userExample);
            groupRepositoryMock.Setup(m => m.GetById(It.IsAny<int>())).Returns(groupExample);

            var result = deckLogic.Unassign(userExample.Token, 1, 1);
        }

        [ExpectedException(typeof(NotFoundException))]
        [TestMethod]
        public void UnassignNoGroupTest()
        {
            userTokenRepository.Setup(m => m.GetUserByToken(It.IsAny<string>())).Returns(userExample);
            groupRepositoryMock.Setup(m => m.GetById(It.IsAny<int>())).Returns((Group)null);
            deckGroupRepositoryMock.Setup(a => a.FindByCondition(It.IsAny<Expression<Func<DeckGroup,
                bool>>>())).Returns(new List<DeckGroup>() { It.IsAny<DeckGroup>() });

            var result = deckLogic.Unassign(userExample.Token, 1, 1);
        }

        [ExpectedException(typeof(NotFoundException))]
        [TestMethod]
        public void UnassignNotFoundGroupTest()
        {
            userTokenRepository.Setup(m => m.GetUserByToken(It.IsAny<string>())).Returns(userExample);
            groupRepositoryMock.Setup(m => m.GetById(It.IsAny<int>())).Returns(groupExample);
            deckGroupRepositoryMock.Setup(a => a.FindByCondition(It.IsAny<Expression<Func<DeckGroup,
                bool>>>())).Returns(new List<DeckGroup>() { });

            var result = deckLogic.Unassign(userExample.Token, 1, 1);
        }

        [TestMethod]
        public void GetFlashcardsCommentsOkTest()
        {
            FlashcardComment flashcardCommentExample = new FlashcardComment()
            {
                Comment = "comment",
                CreatedOn = DateTime.Today,
                CreatorUsername = userExample.Username,
                Id = 1
            };

            Flashcard flashcardExample = new Flashcard()
            {
                Id = 1,
                Question = "This is a question",
                Answer = "This is the answer",
                Comments = new List<FlashcardComment>() { flashcardCommentExample },
                Deck = deckExample,
                UserScores = new List<FlashcardScore>()
            };
            flashcardCommentExample.Flashcard = flashcardExample;

            flashcardRepositoryMock.Setup(f => f.GetById(It.IsAny<int>())).Returns(flashcardExample);
            flashcardCommentRepositoryMock.Setup(a => a.FindByCondition(It.IsAny<Expression<Func<FlashcardComment,
                bool>>>())).Returns(new List<FlashcardComment>() { flashcardCommentExample });

            var result = deckLogic.GetFlashcardsComments(1);
            deckRepositoryMock.VerifyAll();

            Assert.AreEqual(1, result.ToList().Count);
        }

        [ExpectedException(typeof(NotFoundException))]
        [TestMethod]
        public void GetFlashcardsCommentsNoFlashcard()
        {
            FlashcardComment flashcardCommentExample = new FlashcardComment()
            {
                Comment = "comment",
                CreatedOn = DateTime.Today,
                CreatorUsername = userExample.Username,
                Id = 1
            };

            Flashcard flashcardExample = new Flashcard()
            {
                Id = 1,
                Question = "This is a question",
                Answer = "This is the answer",
                Comments = new List<FlashcardComment>() { flashcardCommentExample },
                Deck = deckExample,
                UserScores = new List<FlashcardScore>()
            };
            flashcardCommentExample.Flashcard = flashcardExample;

            flashcardRepositoryMock.Setup(f => f.GetById(It.IsAny<int>())).Returns((Flashcard)null);
            flashcardCommentRepositoryMock.Setup(a => a.FindByCondition(It.IsAny<Expression<Func<FlashcardComment,
                bool>>>())).Returns(new List<FlashcardComment>() { flashcardCommentExample });

            var result = deckLogic.GetFlashcardsComments(1);
        }
    }
}
