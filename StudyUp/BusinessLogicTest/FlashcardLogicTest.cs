﻿using BusinessLogic;
using DataAccessInterface;
using Domain;
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
    public class FlashcardLogicTest
    {
        User userExample;
        Deck deckExample;
        Flashcard flashcardExample;
        Mock<IRepository<Deck>> deckRepositoryMock;
        Mock<IRepository<User>> userRepositoryMock;
        Mock<IRepository<Flashcard>> flashcardRepositoryMock;
        Mock<IRepository<FlashcardScore>> flashcardScoreRepositoryMock;
        Mock<IUserRepository> userTokenRepository;
        FlashcardLogic flashcardLogic;

        [TestInitialize]
        public void SetUp()
        {
            userExample = new User()
            {
                Id = 1,
                Username = "Julia",
                Email = "julia@gmail.com",
                Password = "Julia12-",
                IsStudent = true,
                Token = "token"
            };

            deckExample = new Deck()
            {
                Id = 1,
                Name = "Practicar",
                Author = userExample,
                Difficulty = Domain.Enumerations.Difficulty.Easy,
                IsHidden = false,
                Subject = "Biology",
                Flashcards = new List<Flashcard>()
            };

            flashcardExample = new Flashcard()
            {
                Id = 1,
                Question = "What is the powerhouse of the cell called?",
                Answer = "Mitochondria",
                Deck = deckExample
            };

            deckRepositoryMock = new Mock<IRepository<Deck>>(MockBehavior.Loose);
            userRepositoryMock = new Mock<IRepository<User>>(MockBehavior.Strict);
            flashcardRepositoryMock = new Mock<IRepository<Flashcard>>(MockBehavior.Loose);
            flashcardScoreRepositoryMock = new Mock<IRepository<FlashcardScore>>(MockBehavior.Loose);
            userTokenRepository = new Mock<IUserRepository>(MockBehavior.Strict);
            flashcardLogic = new FlashcardLogic(flashcardRepositoryMock.Object, userRepositoryMock.Object,
                userTokenRepository.Object, deckRepositoryMock.Object, flashcardScoreRepositoryMock.Object);
        }

        [TestMethod]
        public void AddFlashcardOkTest()
        {
            flashcardRepositoryMock.Setup(m => m.Add(It.IsAny<Flashcard>()));
            deckRepositoryMock.Setup(m => m.GetAll()).Returns(new List<Deck>());
            deckRepositoryMock.Setup(m => m.GetById(It.IsAny<int>())).Returns(deckExample);
            userRepositoryMock.Setup(m => m.GetById(It.IsAny<int>())).Returns(userExample);

            userRepositoryMock.Setup(m => m.GetAll()).Returns(new List<User>() { userExample });
            userRepositoryMock.Setup(a => a.Update(It.IsAny<User>()));
            userTokenRepository.Setup(t => t.GetUserByToken(It.IsAny<string>())).Returns(userExample);

            userRepositoryMock.Setup(m => m.Add(It.IsAny<User>()));

            var result = flashcardLogic.AddFlashcard(flashcardExample, flashcardExample.Deck.Id, userExample.Token);

            flashcardRepositoryMock.VerifyAll();
            Assert.AreEqual(flashcardExample, result);
        }

        [ExpectedException(typeof(NotFoundException))]
        [TestMethod]
        public void AddFlashcardNotFoundDeck()
        {
            flashcardRepositoryMock.Setup(m => m.Add(It.IsAny<Flashcard>()));
            deckRepositoryMock.Setup(m => m.GetAll()).Returns(new List<Deck>());
            deckRepositoryMock.Setup(m => m.GetById(It.IsAny<int>())).Returns((Deck)null);
            userRepositoryMock.Setup(m => m.GetById(It.IsAny<int>())).Returns(userExample);

            userRepositoryMock.Setup(m => m.GetAll()).Returns(new List<User>() { userExample });
            userRepositoryMock.Setup(a => a.Update(It.IsAny<User>()));
            userTokenRepository.Setup(t => t.GetUserByToken(It.IsAny<string>())).Returns(userExample);

            userRepositoryMock.Setup(m => m.Add(It.IsAny<User>()));

            var result = flashcardLogic.AddFlashcard(flashcardExample, flashcardExample.Deck.Id, userExample.Token);

            flashcardRepositoryMock.VerifyAll();
            Assert.AreEqual(flashcardExample, result);
        }

        [ExpectedException(typeof(InvalidException))]
        [TestMethod]
        public void AddFlashcardEmptyQuestionTest()
        {
            Flashcard toAdd = new Flashcard()
            {
                Id = 1,
                Deck = deckExample,
                Answer = "1974"
            };

            flashcardRepositoryMock.Setup(m => m.Add(It.IsAny<Flashcard>()));
            deckRepositoryMock.Setup(m => m.GetAll()).Returns(new List<Deck>());
            userRepositoryMock.Setup(m => m.GetById(1)).Returns(userExample);
            userRepositoryMock.Setup(m => m.GetAll()).Returns(new List<User>());
            userRepositoryMock.Setup(a => a.Update(It.IsAny<User>()));

            userRepositoryMock.Setup(m => m.Add(It.IsAny<User>()));

            var result = flashcardLogic.AddFlashcard(toAdd, flashcardExample.Deck.Id, userExample.Token);

            flashcardRepositoryMock.VerifyAll();
        }

        [ExpectedException(typeof(InvalidException))]
        [TestMethod]
        public void AddFlashcardDifferentAuthorTest()
        {
            Flashcard toAdd = new Flashcard()
            {
                Id = 1,
                Deck = deckExample,
                Question = "In what year did the titanic sink?",
                Answer = "1912"
            };

            flashcardRepositoryMock.Setup(m => m.Add(It.IsAny<Flashcard>()));
            deckRepositoryMock.Setup(m => m.GetAll()).Returns(new List<Deck>());
            userRepositoryMock.Setup(m => m.GetById(1)).Returns(userExample);
            userRepositoryMock.Setup(m => m.GetById(3)).Throws(new NotFoundException(UserMessage.USER_NOT_FOUND));
            userRepositoryMock.Setup(m => m.GetAll()).Returns(new List<User>());
            userRepositoryMock.Setup(a => a.Update(It.IsAny<User>()));
            userTokenRepository.Setup(t => t.GetUserByToken(It.IsAny<string>())).Returns(It.IsAny<User>());

            userRepositoryMock.Setup(m => m.Add(It.IsAny<User>()));

            var result = flashcardLogic.AddFlashcard(toAdd, flashcardExample.Deck.Id, userExample.Token);
        }

        [ExpectedException(typeof(InvalidException))]
        [TestMethod]
        public void AddFlashcardInvalidTokenTest()
        {
            Flashcard toAdd = new Flashcard()
            {
                Id = 1,
                Deck = deckExample,
                Question = "In what year did the titanic sink?",
                Answer = "1912"
            };

            flashcardRepositoryMock.Setup(m => m.Add(It.IsAny<Flashcard>()));
            deckRepositoryMock.Setup(m => m.GetAll()).Returns(new List<Deck>());
            userRepositoryMock.Setup(m => m.GetById(1)).Returns(userExample);
            userRepositoryMock.Setup(m => m.GetById(3)).Throws(new NotFoundException(UserMessage.USER_NOT_FOUND));
            userRepositoryMock.Setup(m => m.GetAll()).Returns(new List<User>());
            userRepositoryMock.Setup(a => a.Update(It.IsAny<User>()));
            userTokenRepository.Setup(t => t.GetUserByToken(It.IsAny<string>())).Returns((User) null);

            userRepositoryMock.Setup(m => m.Add(It.IsAny<User>()));

            var result = flashcardLogic.AddFlashcard(toAdd, 1, userExample.Token);
        }

        [TestMethod]
        public void EditFlashcardOkTest()
        {
            Flashcard flashcardAfterEdit = new Flashcard()
            {
                Id = flashcardExample.Id,
                Answer = "new answer",
                Question = "new question",
                Deck = deckExample
            };

            deckRepositoryMock.Setup(m => m.GetAll()).Returns(new List<Deck>());
            userRepositoryMock.Setup(m => m.GetById(It.IsAny<int>())).Returns(userExample);

            flashcardRepositoryMock.Setup(m => m.GetById(1)).Returns(flashcardAfterEdit);
            flashcardRepositoryMock.Setup(f => f.Update(It.IsAny<Flashcard>()));


            userRepositoryMock.Setup(m => m.GetAll()).Returns(new List<User>() { userExample });
            userRepositoryMock.Setup(a => a.Update(It.IsAny<User>()));

            userTokenRepository.Setup(u => u.GetUserByToken(It.IsAny<string>())).Returns(userExample);

            userRepositoryMock.Setup(m => m.Add(It.IsAny<User>()));

            var result = flashcardLogic.EditFlashcard("token", 1, "new question", "new answer");

            flashcardRepositoryMock.VerifyAll();
            Assert.AreEqual(flashcardAfterEdit, result);
        }

        [ExpectedException(typeof(InvalidException))]
        [TestMethod]
        public void EditFlashcardDifferentAuthorTest()
        {
            User anotherUserExample = new User()
            {
                Decks = new List<Deck>(),
                Email = "anotheremail@gmail.com",
                Id = 2,
                IsStudent = false,
                Password = "Password1234",
                Token = "different token",
                Username = "Another User"
            };

            deckRepositoryMock.Setup(m => m.GetAll()).Returns(new List<Deck>());
            flashcardRepositoryMock.Setup(f => f.Update(It.IsAny<Flashcard>()));
            flashcardRepositoryMock.Setup(m => m.GetById(It.IsAny<int>())).Returns(flashcardExample);
            userRepositoryMock.Setup(m => m.GetById(It.IsAny<int>())).Returns(userExample);
            userRepositoryMock.Setup(m => m.GetAll()).Returns(new List<User>() { userExample });
            userRepositoryMock.Setup(a => a.Update(It.IsAny<User>()));
            userTokenRepository.Setup(u => u.GetUserByToken("different token")).Returns(anotherUserExample);
            userRepositoryMock.Setup(m => m.Add(userExample));
            userRepositoryMock.Setup(m => m.Add(anotherUserExample));

            var result = flashcardLogic.EditFlashcard("different token", 1, "new question", "new answer");
        }

        [ExpectedException(typeof(InvalidException))]
        [TestMethod]
        public void DeleteFlashcardDifferentAuthorTest()
        {
            flashcardRepositoryMock.Setup(b => b.GetById(flashcardExample.Id)).Returns(flashcardExample);
            flashcardRepositoryMock.Setup(d => d.Delete(flashcardExample));
            userRepositoryMock.Setup(b => b.GetById(userExample.Id)).Returns(userExample);

            flashcardLogic.DeleteFlashcard(flashcardExample.Id, "different token");
        }

        [ExpectedException(typeof(NotFoundException))]
        [TestMethod]
        public void DeleteFlashcardDoesNotExistTest()
        {
            flashcardRepositoryMock.Setup(b => b.GetById(10)).Returns(It.IsAny<Flashcard>());
            flashcardRepositoryMock.Setup(d => d.Delete(flashcardExample));
            flashcardLogic.DeleteFlashcard(10, userExample.Token);
        }

        [TestMethod]
        public void DeleteFlashcardOkTest()
        {
            flashcardRepositoryMock.Setup(b => b.GetById(flashcardExample.Id)).Returns(flashcardExample);
            userRepositoryMock.Setup(b => b.GetById(userExample.Id)).Returns(userExample);
            userRepositoryMock.Setup(d => d.Update(userExample));

            var result = flashcardLogic.DeleteFlashcard(flashcardExample.Id, userExample.Token);

            flashcardRepositoryMock.VerifyAll();
            Assert.IsTrue(result);
        }

        [ExpectedException(typeof(NotFoundException))]
        [TestMethod]
        public void GetRatedFlashcardsNullDeckTest()
        {
            deckRepositoryMock.Setup(d => d.GetById(It.IsAny<int>())).Returns((Deck)null);
            userTokenRepository.Setup(b => b.GetUserByToken(It.IsAny<string>())).Returns(userExample);
            flashcardScoreRepositoryMock.Setup(fs => fs.FindByCondition(It.IsAny<Expression<Func<FlashcardScore, bool>>>()))
                .Returns(new List<FlashcardScore>());

            var result = flashcardLogic.GetRatedFlashcards(1, "Token");

            deckRepositoryMock.VerifyAll();
        }

        [ExpectedException(typeof(InvalidException))]
        [TestMethod]
        public void GetRatedFlashcardsNullUserTest()
        {
            deckRepositoryMock.Setup(d => d.GetById(It.IsAny<int>())).Returns(deckExample);
            userTokenRepository.Setup(b => b.GetUserByToken(It.IsAny<string>())).Returns((User)null);
            flashcardScoreRepositoryMock.Setup(fs => fs.FindByCondition(It.IsAny<Expression<Func<FlashcardScore, bool>>>()))
                .Returns(new List<FlashcardScore>());

            var result = flashcardLogic.GetRatedFlashcards(1, "Token");

            deckRepositoryMock.VerifyAll();
        }

        [TestMethod]
        public void GetRatedFlashcardsWithoutScoreTest()
        {
            deckExample.Flashcards = new List<Flashcard>() { flashcardExample };
            deckRepositoryMock.Setup(d => d.GetById(It.IsAny<int>())).Returns(deckExample);
            userTokenRepository.Setup(b => b.GetUserByToken(It.IsAny<string>())).Returns(userExample);
            flashcardScoreRepositoryMock.Setup(fs => fs.FindByCondition(It.IsAny<Expression<Func<FlashcardScore, bool>>>()))
                .Returns(new List<FlashcardScore>());

            var result = flashcardLogic.GetRatedFlashcards(1, "Token");

            deckRepositoryMock.VerifyAll();

            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(flashcardExample, result[0].Item1);
            Assert.AreEqual(0, result[0].Item2);
        }

        [TestMethod]
        public void GetRatedFlashcardsWithScoreTest()
        {
            deckExample.Flashcards = new List<Flashcard>() { flashcardExample };
            deckRepositoryMock.Setup(d => d.GetById(It.IsAny<int>())).Returns(deckExample);
            userTokenRepository.Setup(b => b.GetUserByToken(It.IsAny<string>())).Returns(userExample);
            flashcardScoreRepositoryMock.Setup(fs => fs.FindByCondition(It.IsAny<Expression<Func<FlashcardScore, bool>>>()))
                .Returns(new List<FlashcardScore>() { new FlashcardScore() { Flashcard = flashcardExample, Score = 10 } });

            var result = flashcardLogic.GetRatedFlashcards(1, "Token");

            deckRepositoryMock.VerifyAll();

            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(flashcardExample, result[0].Item1);
            Assert.AreEqual(10, result[0].Item2);
        }

        [ExpectedException(typeof(NotFoundException))]
        [TestMethod]
        public void UpdateFlascardScoreNullFlashcardTest()
        {
            flashcardRepositoryMock.Setup(d => d.GetById(It.IsAny<int>())).Returns((Flashcard)null);
            userTokenRepository.Setup(b => b.GetUserByToken(It.IsAny<string>())).Returns(userExample);
            flashcardScoreRepositoryMock.Setup(fs => fs.FindByCondition(It.IsAny<Expression<Func<FlashcardScore, bool>>>()))
                .Returns(new List<FlashcardScore>());

            var result = flashcardLogic.UpdateScore(1, 5, "Token");

            flashcardRepositoryMock.VerifyAll();
        }

        [ExpectedException(typeof(InvalidException))]
        [TestMethod]
        public void UpdateFlascardScoreNullUserTest()
        {
            flashcardRepositoryMock.Setup(d => d.GetById(It.IsAny<int>())).Returns(flashcardExample);
            userTokenRepository.Setup(b => b.GetUserByToken(It.IsAny<string>())).Returns((User)null);
            flashcardScoreRepositoryMock.Setup(fs => fs.FindByCondition(It.IsAny<Expression<Func<FlashcardScore, bool>>>()))
                .Returns(new List<FlashcardScore>());

            var result = flashcardLogic.UpdateScore(1, 5, "Token");

            flashcardRepositoryMock.VerifyAll();
            userTokenRepository.VerifyAll();
        }

        [TestMethod]
        public void UpdateFlascardScoreNonExistingTest()
        {
            flashcardExample.UserScores = new List<FlashcardScore>();
            flashcardRepositoryMock.Setup(d => d.GetById(It.IsAny<int>())).Returns(flashcardExample);
            userTokenRepository.Setup(b => b.GetUserByToken(It.IsAny<string>())).Returns(userExample);
            flashcardScoreRepositoryMock.Setup(fs => fs.FindByCondition(It.IsAny<Expression<Func<FlashcardScore, bool>>>()))
                .Returns(new List<FlashcardScore>());

            var result = flashcardLogic.UpdateScore(1, 5, "Token");

            flashcardRepositoryMock.VerifyAll();
            userTokenRepository.VerifyAll();
            flashcardScoreRepositoryMock.VerifyAll();

            Assert.AreEqual(1, result.UserScores.Count());
            Assert.AreEqual(5, result.UserScores[0].Score);
        }

        [TestMethod]
        public void UpdateFlascardScoreExistingTest()
        {
            var flashcardScoreExample = new FlashcardScore() { UserId=1,  FlashcardId = flashcardExample.Id, Flashcard = flashcardExample, Score = 10 };
            flashcardExample.UserScores = new List<FlashcardScore>() { flashcardScoreExample };
            flashcardRepositoryMock.Setup(d => d.GetById(It.IsAny<int>())).Returns(flashcardExample);
            deckRepositoryMock.Setup(d => d.GetById(It.IsAny<int>())).Returns(deckExample);
            userTokenRepository.Setup(b => b.GetUserByToken(It.IsAny<string>())).Returns(userExample);
            flashcardScoreRepositoryMock.Setup(fs => fs.FindByCondition(It.IsAny<Expression<Func<FlashcardScore, bool>>>()))
                .Returns(new List<FlashcardScore>() {  flashcardScoreExample });

            var result = flashcardLogic.UpdateScore(1, 5, "Token");

            flashcardRepositoryMock.VerifyAll();
            userTokenRepository.VerifyAll();
            flashcardScoreRepositoryMock.VerifyAll();

            Assert.AreEqual(1, result.UserScores.Count());
            Assert.AreEqual(5, result.UserScores[0].Score);
        }

    }
}