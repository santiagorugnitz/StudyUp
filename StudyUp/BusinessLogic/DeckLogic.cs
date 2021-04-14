using BusinessLogicInterface;
using DataAccessInterface;
using Domain;
using Domain.Enumerations;
using Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLogic
{
    public class DeckLogic : IDeckLogic
    {
        private IRepository<Deck> deckRepository;
        private IRepository<User> userRepository;
        private IUserRepository userTokenRepository;

        public DeckLogic(IRepository<Deck> repository, IRepository<User> userRepository,
            IUserRepository userTokenRepository)
        {
            this.deckRepository = repository;
            this.userRepository = userRepository;
            this.userTokenRepository = userTokenRepository;
        }

        public Deck AddDeck(Deck deck, string userToken)
        {
            IEnumerable<Deck> sameName = deckRepository.GetAll().Where(x => x.Name.Equals(deck.Name));
            if (sameName != null && (sameName.Count() > 0))
                throw new AlreadyExistsException(DeckMessage.DECK_ALREADY_EXISTS);
            else if (deck.Name is null)
                throw new InvalidException(DeckMessage.EMPTY_NAME_MESSAGE);
            else if ((int)deck.Difficulty < 0 || (int)deck.Difficulty > 2)
                throw new InvalidException(DeckMessage.INVALID_DIFFICULTY_MESSAGE);
            else if (deck.Subject is null)
                throw new InvalidException(DeckMessage.EMPTY_SUBJECT_MESSAGE);

            deckRepository.Add(deck);
            User user = userTokenRepository.GetUserByToken(userToken);
            User userById = userRepository.GetById(user.Id);
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


        public Deck EditDeck(int deckId, string newName, Difficulty newDifficulty, bool newVisibility)
        {
            Deck deck = deckRepository.GetById(deckId);
            ICollection<Deck> sameName = deckRepository.FindByCondition(a => a.Name == newName && a.Id != deckId);
            if (sameName != null && sameName.Count > 0)
                throw new AlreadyExistsException(DeckMessage.DECK_ALREADY_EXISTS);
            if ((int)newDifficulty > 2 || (int)newDifficulty < 0)
                throw new InvalidException(DeckMessage.INVALID_DIFFICULTY);
            if (deck != null)
            {
                deck.Name = newName;
                deck.Difficulty = newDifficulty;
                deck.IsHidden = newVisibility;
                deckRepository.Update(deck);
                return deck;
            }
            else throw new NotFoundException(DeckMessage.DECK_NOT_FOUND);
        }
    }
}
