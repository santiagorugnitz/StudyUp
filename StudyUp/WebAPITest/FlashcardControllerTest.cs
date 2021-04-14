﻿using BusinessLogicInterface;
using Domain;
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
    public class FlashcardControllerTest
    {
        Mock<IFlashcardLogic> logicMock;
        FlashcardController controller;
        FlashcardModel flashcardModelExample;
        DeckModel deckModelExample;
        UserModel userModelExample;

        [TestInitialize]
        public void SetUp()
        {
            logicMock = new Mock<IFlashcardLogic>(MockBehavior.Strict);
            controller = new FlashcardController(logicMock.Object);


            userModelExample = new UserModel()
            {
                Id = 1,
                Username = "Gabriel",
                IsStudent = false,
                Email = "gabriel@hotmail.com",
                Password = "contraseña123-",
            };

            deckModelExample = new DeckModel()
            {
                Name = "Clase de repaso",
                Author = userModelExample.ToEntity(),
                Difficulty = Domain.Enumerations.Difficulty.Easy,
                IsHidden = false,
                Subject = "Russian"
            };

            flashcardModelExample = new FlashcardModel()
            {
                Id = 1,
                Question = "who is the president of the united states?",
                Answer = "Joe Biden",
                Deck = deckModelExample.ToEntity()
            };
        }

        [TestMethod]
        public void PostFlashcardOkTest()
        {
            logicMock.Setup(x => x.AddFlashcard(It.IsAny<Flashcard>(), It.IsAny<string>())).Returns(new FlashcardModel().ToEntity());

            var result = controller.Post(flashcardModelExample, userModelExample.Token);
            var okResult = result as OkObjectResult;
            var value = okResult.Value as Flashcard;

            logicMock.VerifyAll();
        }

        [TestMethod]
        public void EditFlashcardOkTest()
        {
            EditFlashcardModel editFlashcardModel = new EditFlashcardModel()
            {
                Id = flashcardModelExample.Id,
                Answer = "new answer",
                Question = "new question"
            };

            logicMock.Setup(x => x.EditFlashcard(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(),
                It.IsAny<string>())).Returns(new FlashcardModel().ToEntity());

            var result = controller.EditFlashcard("token", editFlashcardModel);
            var okResult = result as OkObjectResult;
            var value = okResult.Value as Flashcard;

            logicMock.VerifyAll();

        }
    }
}
