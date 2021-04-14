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
        public IActionResult Post([FromBody] FlashcardModel flashcardModel, [FromQuery] int userId)
        {
            Flashcard newFlashcard = logic.AddFlashcard(flashcardModel.ToEntity(), userId);
            return Ok(newFlashcard);
        }

        [HttpPut("edit-flashcard")]
        public IActionResult EditFlashcard([FromHeader] string token,
            [FromBody] EditFlashcardModel editFlashcardModel)
        {
            return Ok(logic.EditFlashcard(token, editFlashcardModel.Id, editFlashcardModel.Question,
                editFlashcardModel.Answer));
        }
    }
}
