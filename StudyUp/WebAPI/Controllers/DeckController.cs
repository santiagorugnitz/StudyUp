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
        public IActionResult Post([FromBody] DeckModel deckModel)
        {
            try
            {
                Deck newDeck = logic.AddDeck(deckModel.ToEntity());
                return Ok(newDeck);
            }
            catch (AlreadyExistsException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
