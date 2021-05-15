using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogicInterface;
using Domain;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Filters;
using WebAPI.Models;

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
            return Ok(newFlashcard);
        }

        [HttpPut("{id}")]
        public IActionResult EditFlashcard([FromRoute] int id, [FromHeader] string token,
            [FromBody] EditFlashcardModel editFlashcardModel)
        {
            return Ok(logic.EditFlashcard(token, id, editFlashcardModel.Question,
                editFlashcardModel.Answer));
        }

        [HttpPost("study")]
        public IActionResult EditScore([FromHeader] string token, [FromBody] List<SingleFlashcardModel> scoredFlashcards)
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

        [HttpPost("{id}/comment")]
        public IActionResult CommentFlashcard([FromRoute] int id,[FromHeader] string token, [FromBody] string comment)
        {
            logic.CommentFlashcard(id, token, comment);
           
            return Ok();
        }
    }
}
