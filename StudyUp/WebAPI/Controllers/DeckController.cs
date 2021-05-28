using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using BusinessLogicInterface;
using Domain;
using WebAPI.Models;
using WebAPI.Filters;
using WebAPI.Models.ResponseModels;

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
            if (userId > 0)
            {
                return Ok(ConvertDecks(logic.GetDecksByAuthor(userId)));
            }

            return Ok(ConvertDecks(logic.GetAllDecks()));
        }

        private IEnumerable<ResponseDeckModel> ConvertDecks(IEnumerable<Deck> decksList)
        {
            return decksList.Select(deck => new ResponseDeckModel()
            {
                Id = deck.Id,
                Author = deck.Author.Username,
                Name = deck.Name,
                Subject = deck.Subject,
                Difficulty = deck.Difficulty,
                IsHidden = deck.IsHidden,
                FlashcardCount = deck.Flashcards.Count
            });
        }

        [HttpPost]
        public IActionResult Post([FromBody] DeckModel deckModel, [FromHeader] string token)
        {
            Deck deck = logic.AddDeck(deckModel.ToEntity(), token);
            return Ok(new ResponseDeckModel()
            {
                Id = deck.Id,
                Author = deck.Author.Username,
                Name = deck.Name,
                Subject = deck.Subject,
                Difficulty = deck.Difficulty,
                IsHidden = deck.IsHidden
            });
        }

        [HttpPut("{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] UpdateDeckModel updateDeckModel)
        {
            Deck deck = logic.EditDeck(id, updateDeckModel.Name,
                updateDeckModel.Difficulty, updateDeckModel.IsHidden, updateDeckModel.Subject);
            return Ok(new ResponseDeckModel()
            {
                Id = deck.Id,
                Author = deck.Author.Username,
                Name = deck.Name,
                Subject = deck.Subject,
                Difficulty = deck.Difficulty,
                IsHidden = deck.IsHidden
            });
        }

        [HttpGet("{id}")]
        public IActionResult GetDeckById([FromRoute] int id)
        {
            Deck deck = logic.GetDeckById(id);
            return Ok(new ResponseFullDeckModel()
            {
                Id = deck.Id,
                Author = deck.Author.Username,
                Name = deck.Name,
                Subject = deck.Subject,
                Difficulty = deck.Difficulty,
                IsHidden = deck.IsHidden,
                Flashcards = deck.Flashcards.Select(flashcard => new ResponseFlashcardModel()
                {
                    Id = flashcard.Id,
                    Question = flashcard.Question,
                    Answer = flashcard.Answer,
                    Comments = ListFlashcardsComments(flashcard.Id)
                })
            });
        }

        private IEnumerable<ResponseFlashcardCommentsModel> ListFlashcardsComments(int flashcardsId)
        {
            IEnumerable<FlashcardComment> flashcardsComments = logic.GetFlashcardsComments(flashcardsId);
            List<ResponseFlashcardCommentsModel> toReturn = new List<ResponseFlashcardCommentsModel>();

            foreach (FlashcardComment comment in flashcardsComments)
            {
                ResponseFlashcardCommentsModel toAdd = new ResponseFlashcardCommentsModel()
                {
                    Comment = comment.Comment,
                    Id = comment.Id,
                    Time = comment.CreatedOn.ToUniversalTime(),
                    AuthorUsername = comment.CreatorUsername
                };
                toReturn.Add(toAdd);
            }
            return toReturn;
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id, [FromHeader] string token)
        {
            logic.DeleteDeck(id, token);
            return Ok();
        }

        [HttpPost("{deckId}/assign")]
        public IActionResult Assign([FromHeader] string token, [FromRoute] int deckId,
            [FromQuery] int groupId)
        {
            logic.Assign(token, groupId, deckId);
            return Ok();
        }

        [HttpDelete("{deckId}/unassign")]
        public IActionResult Unassign([FromHeader] string token, [FromRoute] int deckId,
            [FromQuery] int groupId)
        {
            logic.Unassign(token, groupId, deckId);
            return Ok();
        }
    }
}
