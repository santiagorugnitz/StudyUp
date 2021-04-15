using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BusinessLogicInterface;
using Exceptions;
using Domain;
using WebAPI.Models;
using WebAPI.Filters;

namespace WebAPI.Controllers
{
    [ServiceFilter(typeof(ExceptionFilter))]
    [Route("api/decks")]
    [ApiController]
    public class DeckController : ControllerBase
    {
        private readonly IDeckLogic logic;

        public DeckController(IDeckLogic logic)
        {
            this.logic = logic;
        }

        [HttpGet]
        public IActionResult GetAllDecks()
        {
            return Ok(logic.GetAllDecks());
        }

        [HttpPost]
        public IActionResult Post([FromBody] DeckModel deckModel, [FromHeader] string token)
        {
            Deck newDeck = logic.AddDeck(deckModel.ToEntity(), token);
            return Ok(newDeck);
        }

        [HttpGet("byAuthor")]
        public IActionResult GetDecksByAuthor([FromQuery] int userId)
        {
            IEnumerable<Deck> decksList = logic.GetDecksByAuthor(userId);
            return Ok(decksList);
        }

        [HttpPut]
        public IActionResult Update([FromQuery] int deckId, [FromBody] UpdateDeckModel updateDeckModel)
        {
            return Ok(logic.EditDeck(deckId, updateDeckModel.Name,
                updateDeckModel.Difficulty, updateDeckModel.IsHidden));
        }

        [HttpGet("{id}")]
        public IActionResult GetDeckById([FromQuery] int deckId)
        {
            Deck deck = logic.GetDeckById(deckId);
            return Ok(deck);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromQuery] int deckId, [FromHeader] string token)
        {
            logic.DeleteDeck(deckId, token);
            return Ok("Successfully deleted.");
        }
    }
}
