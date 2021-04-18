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
        public IActionResult GetAllDecks([FromQuery] int userId = -1)
        {
            IEnumerable<Deck> decksList;
            
            if (userId > 0)
            {
                decksList = logic.GetDecksByAuthor(userId);
            } 
            else
            {
                decksList = logic.GetAllDecks();
            }
             
            return Ok(decksList);
        }

        [HttpPost]
        public IActionResult Post([FromBody] DeckModel deckModel, [FromHeader] string token)
        {
            Deck newDeck = logic.AddDeck(deckModel.ToEntity(), token);
            return Ok(newDeck);
        }

        [HttpPut("{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] UpdateDeckModel updateDeckModel)
        {
            return Ok(logic.EditDeck(id, updateDeckModel.Name,
                updateDeckModel.Difficulty, updateDeckModel.IsHidden, updateDeckModel.Subject));
        }

        [HttpGet("{id}")]
        public IActionResult GetDeckById([FromRoute] int id)
        {
            Deck deck = logic.GetDeckById(id);
            return Ok(deck);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id, [FromHeader] string token)
        {
            logic.DeleteDeck(id, token);
            return Ok("Successfully deleted.");
        }
    }
}
