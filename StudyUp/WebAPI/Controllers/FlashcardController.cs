using System.Collections.Generic;
using BusinessLogicInterface;
using Domain;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Filters;
using WebAPI.Models;
using WebAPI.Models.RequestModels;

namespace WebAPI.Controllers
{
    [ServiceFilter(typeof(ExceptionFilter))]
    [Route("api/flashcards")]
    [ApiController]
    public class FlashcardController : Controller
    {
        private readonly IFlashcardLogic logic;

        public FlashcardController(IFlashcardLogic logic)
        {
            this.logic = logic;
        }

        [HttpPost]
        public IActionResult Post([FromBody] FlashcardModel flashcardModel, [FromHeader] string token)
        {
            Flashcard newFlashcard = logic.AddFlashcard(flashcardModel.ToEntity(), flashcardModel.DeckId, token);

            var result = Ok(new ResponseFlashcardModel { Id = newFlashcard.Id, Answer = newFlashcard.Answer, Question = newFlashcard.Question });
            return result;
        }

        [HttpPut("{id}")]
        public IActionResult EditFlashcard([FromRoute] int id, [FromHeader] string token,
            [FromBody] EditFlashcardModel editFlashcardModel)
        {
            Flashcard editedFlashcard = new Flashcard()
            {
                Answer = editFlashcardModel.Answer,
                Question = editFlashcardModel.Question
            };

            var result = logic.EditFlashcard(token, id, editedFlashcard);

            return Ok(new ResponseFlashcardModel
            {
                Id = result.Id,
                Answer = result.Answer,
                Question = result.Question
            });
        }

        [HttpPost("study")]
        public IActionResult EditScore([FromHeader] string token, 
            [FromBody] List<SingleFlashcardModel> scoredFlashcards)
        {
            foreach (var scoredFlashcard in scoredFlashcards)
            {
                logic.UpdateScore(scoredFlashcard.FlashcardId, scoredFlashcard.Score, token);
            }

            return Ok();
        }

        [HttpGet("rated")]
        public IActionResult GetRatedFlashcards([FromQuery] int deckId, [FromHeader] string token)
        {
            List<ResponseFlashcardScoreModel> returningList = new List<ResponseFlashcardScoreModel>();
            var gotFlashcards = logic.GetRatedFlashcards(deckId, token);

            foreach (var ratedFlashcard in gotFlashcards)
            {
                returningList.Add(new ResponseFlashcardScoreModel()
                {
                    Id = ratedFlashcard.Item1.Id,
                    Question = ratedFlashcard.Item1.Question,
                    Answer = ratedFlashcard.Item1.Answer,
                    Score = ratedFlashcard.Item2
                });
            }

            return Ok(returningList);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id, [FromHeader] string token)
        {
            logic.DeleteFlashcard(id, token);
            return Ok();
        }

        [HttpPost("{id}/comments")]
        public IActionResult CommentFlashcard([FromRoute] int id, [FromHeader] string token, [FromBody] CommentModel comment)
        {
            logic.CommentFlashcard(id, token, comment.Comment);

            return Ok();
        }

        [HttpDelete("{flashcardId}/comments/{commentId}")]
        public IActionResult DeleteComment([FromHeader] string token, [FromRoute] int flashcardId, [FromRoute] int commentId)
        {
            logic.DeleteComment(token, flashcardId, commentId);
            return Ok();
        }

    }
}
