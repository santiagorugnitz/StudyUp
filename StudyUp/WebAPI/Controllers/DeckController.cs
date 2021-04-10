using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BusinessLogicInterface;
using Exceptions;
using Domain;
using WebAPI.Models;

namespace WebAPI.Controllers
{
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
            try
            {
                return Ok(logic.GetAllDecks());
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] DeckModel deckModel, [FromQuery] int userId)
        {
            try
            {
                Deck newDeck = logic.AddDeck(deckModel.ToEntity(), userId);
                return Ok(newDeck);
            }
            catch (AlreadyExistsException e)
            {
                return BadRequest(e.Message);
            }
            catch (InvalidException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("byAuthor")]
        public IActionResult GetDecksByAuthor([FromQuery] int userId)
        {
            try
            {
                IEnumerable<Deck> decksList = logic.GetDecksByAuthor(userId);
                return Ok(decksList);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPut]
        public IActionResult Update([FromQuery] int deckId, [FromBody] UpdateDeckModel updateDeckModel)
        {
            try
            {
                return Ok(logic.EditDeck(deckId, updateDeckModel.Name, 
                    updateDeckModel.Difficulty, updateDeckModel.IsHidden));
            }
            catch (InvalidException e)
            {
                return BadRequest(e.Message);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
