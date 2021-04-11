﻿using System;
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
        public IActionResult Post([FromBody] DeckModel deckModel, [FromQuery] int userId)
        {
            Deck newDeck = logic.AddDeck(deckModel.ToEntity(), userId);
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
    }
}
