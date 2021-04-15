using Domain;
using Domain.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogicInterface
{
    public interface IDeckLogic
    {
        Deck AddDeck(Deck deck, string userToken);
        IEnumerable<Deck> GetAllDecks();
        IEnumerable<Deck> GetDecksByAuthor(int userId);
        Deck EditDeck(int deckId, string newName, Difficulty newDifficulty, bool newVisibility);
        Deck GetDeckById(int deckId);
    }
}
