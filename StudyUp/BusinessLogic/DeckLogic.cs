using BusinessLogicInterface;
using DataAccessInterface;
using Domain;
using Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLogic
{
    public class DeckLogic:IDeckLogic
    {
        private IRepository<Deck> repository;

        public DeckLogic(IRepository<Deck> repository)
        {
            this.repository = repository;
        }

        public Deck AddDeck(Deck deck)
        {
            IEnumerable<Deck> sameName = repository.GetAll().Where(x => x.Name.Equals(deck.Name));
            if (sameName != null && (sameName.Count() > 0))
                throw new AlreadyExistsException(DeckMessage.DECK_ALREADY_EXISTS);

            repository.Add(deck);
            return deck;
        }

        public IEnumerable<Deck> GetAllDecks()
        {
            return repository.GetAll();
        }
    }
}
