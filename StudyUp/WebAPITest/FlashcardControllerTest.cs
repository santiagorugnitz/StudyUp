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
        Flashcard flashcardExample;
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
                Difficulty = Domain.Enumerations.Difficulty.Easy,
                IsHidden = false,
                Subject = "Russian"
            };

            flashcardModelExample = new FlashcardModel()
            {
                DeckId = 1,
                Question = "who is the president of the united states?",
                Answer = "Joe Biden"
            };

            flashcardExample = new Flashcard()
            {
                Question = "who is the president of the united states?",
                Answer = "Joe Biden"
            };
        }

        [TestMethod]
        public void PostFlashcardOkTest()
        {
            logicMock.Setup(x => x.AddFlashcard(It.IsAny<Flashcard>(), It.IsAny<int>(), It.IsAny<string>())).Returns(new FlashcardModel().ToEntity());

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
                Answer = "new answer",
                Question = "new question"
            };

            logicMock.Setup(x => x.EditFlashcard(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(),
                It.IsAny<string>())).Returns(new FlashcardModel().ToEntity());

            var result = controller.EditFlashcard(1, "token", editFlashcardModel);
            var okResult = result as OkObjectResult;
            var value = okResult.Value as Flashcard;

            logicMock.VerifyAll();
        }

        [TestMethod]
        public void DeleteFlashcardOkTest()
        {
            logicMock.Setup(x => x.DeleteFlashcard(It.IsAny<int>(), It.IsAny<string>())).Returns(true);

            var result = controller.Delete(flashcardModelExample.ToEntity().Id, userModelExample.Token);
            var okResult = result as OkObjectResult;

            logicMock.VerifyAll();
        }

        [TestMethod]
        public void GetRatedFlashcardOkTest()
        {
            logicMock.Setup(x => x.GetRatedFlashcards(It.IsAny<int>(), It.IsAny<string>())).Returns(new List<Tuple<Flashcard, int>>() 
            { new Tuple<Flashcard, int>(flashcardExample, 10) });

            var result = controller.GetRatedFlashcards(flashcardModelExample.ToEntity().Id, userModelExample.Token);
            var okResult = result as OkObjectResult;
            var value = okResult.Value as List<ResponseFlashcardScoreModel>;

            logicMock.VerifyAll();
        }

        [TestMethod]
        public void UpateRateFlashcardOkTest()
        {
            logicMock.Setup(x => x.UpdateScore(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).Returns(flashcardExample);

            var singleModel = new SingleFlashcardModel { FlashcardId = flashcardModelExample.ToEntity().Id, Score = 10 };
            var result = controller.EditScore(userModelExample.Token, new List<SingleFlashcardModel> { singleModel });
            var okResult = result as OkObjectResult;

            logicMock.VerifyAll();
        }
    }
}