using Domain;
using Domain.Enumerations;
using System.Collections.Generic;

namespace BusinessLogicInterface
{
    public interface IDeckLogic
    {
        Deck AddDeck(Deck deck, string userToken);
        IEnumerable<Deck> GetAllDecks();
        IEnumerable<Deck> GetDecksByAuthor(int userId);
        Deck EditDeck(int deckId, string newName, Difficulty newDifficulty, bool newVisibility, string subject);
        Deck GetDeckById(int deckId);
        bool DeleteDeck(int deckId, string token);
        Group Assign(string token, int groupId, int deckId);
        Group Unassign(string token, int groupId, int deckId);
        IEnumerable<FlashcardComment> GetFlashcardsComments(int flashcardId);
    }
}
