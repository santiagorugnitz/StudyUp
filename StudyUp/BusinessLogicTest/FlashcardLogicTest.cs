﻿using BusinessLogic;
using DataAccessInterface;
using Domain;
using Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
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
                IsStudent = true
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

            deckRepositoryMock = new Mock<IRepository<Deck>>(MockBehavior.Strict);
            userRepositoryMock = new Mock<IRepository<User>>(MockBehavior.Strict);
            flashcardRepositoryMock = new Mock<IRepository<Flashcard>>(MockBehavior.Strict);
            flashcardLogic = new FlashcardLogic(flashcardRepositoryMock.Object, userRepositoryMock.Object);
        }

        [TestMethod]
        public void AddFlashcardOkTest()
        {
            flashcardRepositoryMock.Setup(m => m.Add(It.IsAny<Flashcard>()));
            deckRepositoryMock.Setup(m => m.GetAll()).Returns(new List<Deck>());
            userRepositoryMock.Setup(m => m.GetById(It.IsAny<int>())).Returns(userExample);

            userRepositoryMock.Setup(m => m.GetAll()).Returns(new List<User>() { userExample});
            userRepositoryMock.Setup(a => a.Update(It.IsAny<User>()));

            userRepositoryMock.Setup(m => m.Add(It.IsAny<User>()));

            var result = flashcardLogic.AddFlashcard(flashcardExample, 1);

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

            var result = flashcardLogic.AddFlashcard(toAdd, userExample.Id);

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

            userRepositoryMock.Setup(m => m.Add(It.IsAny<User>()));

            var result = flashcardLogic.AddFlashcard(toAdd, 3);

            flashcardRepositoryMock.VerifyAll();
        }


    }
}
