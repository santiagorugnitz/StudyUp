using Domain;
using Domain.Enumerations;
using System.Collections.Generic;

namespace BusinessLogicInterface
{
    public interface IDeckLogic
    {
        Deck AddDeck(Deck deck, string userToken);
        Group Assign(string token, int groupId, int deckId);
        bool DeleteDeck(int deckId, string token);
        Deck EditDeck(int deckId, string newName, Difficulty newDifficulty, bool newVisibility,
            string subject);
        IEnumerable<Deck> GetAllDecks();
        IEnumerable<Deck> GetDecksByAuthor(int userId);
        Deck GetDeckById(int deckId);
        IEnumerable<FlashcardComment> GetFlashcardsComments(int flashcardId);
        Group Unassign(string token, int groupId, int deckId);
    }
}
