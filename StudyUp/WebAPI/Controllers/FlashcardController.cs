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

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id, [FromHeader] string token)
        {
            logic.DeleteFlashcard(id, token);
            return Ok();
        }
    }
}
