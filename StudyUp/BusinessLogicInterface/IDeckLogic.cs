using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogicInterface
{
    public interface IDeckLogic
    {
        Deck AddDeck(Deck deck, int userId);
        IEnumerable<Deck> GetAllDecks();
        IEnumerable<Deck> GetDecksByAuthor(int userId);
    }
}
