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

        [HttpPost("{id}/study")]
        public IActionResult EditScore([FromRoute] int id, [FromQuery] int score, [FromHeader] string token)
        {
            return Ok(logic.UpdateScore(id, score, token));
        }

        [HttpGet("rated")]
        public IActionResult GetRatedFlashcards([FromQuery] int deckId, [FromHeader] string token)
        {
            List<ResponseFlashcardScoreModel> returningList = new List<ResponseFlashcardScoreModel>();
            var gotFlashcards = logic.GetRatedFlashcards(deckId, token);
            
            foreach (var ratedFlashcard in gotFlashcards)
            {
                returningList.Add(new ResponseFlashcardScoreModel() { Flashcard = ratedFlashcard.Item1, Score = ratedFlashcard.Item2 });
            }

            return Ok(returningList);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id, [FromHeader] string token)
        {
            logic.DeleteFlashcard(id, token);
            return Ok();
        }
    }
}
