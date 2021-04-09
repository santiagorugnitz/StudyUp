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
        private IRepository<Deck> deckRepository;
        private IRepository<User> userRepository;


        public DeckLogic(IRepository<Deck> repository, IRepository<User> userRepository)
        {
            this.deckRepository = repository;
            this.userRepository = userRepository;
        }

        public Deck AddDeck(Deck deck, int userId)
        {
            IEnumerable<Deck> sameName = deckRepository.GetAll().Where(x => x.Name.Equals(deck.Name));
            if (sameName != null && (sameName.Count() > 0))
                throw new AlreadyExistsException(DeckMessage.DECK_ALREADY_EXISTS);
            else if (deck.Name is null)
                throw new InvalidException(DeckMessage.EMPTY_NAME_MESSAGE);
            deckRepository.Add(deck);

            User user = userRepository.GetById(userId);
            userRepository.Update(user);
            return deck;
        }

        public IEnumerable<Deck> GetAllDecks()
        {
            return deckRepository.GetAll();
        }

        public IEnumerable<Deck> GetDecksByAuthor(int userId)
        {
            ICollection<Deck> toReturn = new List<Deck>();
            var authorsDecks = deckRepository.FindByCondition(t => t.Author.Id == userId);
            return authorsDecks;
        }
    }
}
