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
        private IRepository<Flashcard> flashcardRepository;
        private IUserRepository userTokenRepository;

        public DeckLogic(IRepository<Deck> repository, IRepository<User> userRepository,
            IUserRepository userTokenRepository, IRepository<Flashcard> flashcardRepository)
        {
            this.deckRepository = repository;
            this.userRepository = userRepository;
            this.userTokenRepository = userTokenRepository;
            this.flashcardRepository = flashcardRepository;
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

            User user = userTokenRepository.GetUserByToken(userToken);
            if (user == null)
            {
                throw new NotAuthenticatedException(UnauthenticatedMessage.UNAUTHENTICATED_USER);
            }
              
            deck.Author = user;
            deckRepository.Add(deck);

            user.Decks.Add(deck);
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
            if (userRepository.GetById(userId) == null)
                throw new NotFoundException(UserMessage.USER_NOT_FOUND);

            var authorsDecks = deckRepository.FindByCondition(t => t.Author.Id == userId);
            return authorsDecks;
        }


        public Deck EditDeck(int deckId, string newName, Difficulty newDifficulty, bool newVisibility, string subject)
        {
            Deck deck = deckRepository.GetById(deckId);
            ICollection<Deck> sameName = deckRepository.FindByCondition(a => a.Name == newName && a.Id != deckId);
            if (sameName != null && sameName.Count > 0)
                throw new AlreadyExistsException(DeckMessage.DECK_ALREADY_EXISTS);
            if ((int)newDifficulty > 2 || (int)newDifficulty < 0)
                throw new InvalidException(DeckMessage.INVALID_DIFFICULTY);
            if (subject is null || subject.Trim().Length == 0)
                throw new InvalidException(DeckMessage.EMPTY_SUBJECT_MESSAGE);
            
            if (deck != null)
            {
                deck.Name = newName;
                deck.Difficulty = newDifficulty;
                deck.IsHidden = newVisibility;
                deck.Subject = subject;
                deckRepository.Update(deck);
                return deck;
            }

            else throw new NotFoundException(DeckMessage.DECK_NOT_FOUND);
        }

        public Deck GetDeckById(int deckId)
        {
            Deck deck = this.deckRepository.GetById(deckId);
            if (deck != null)
                return deck;
            else
                throw new NotFoundException(DeckMessage.DECK_NOT_FOUND);
        }

        public bool DeleteDeck(int deckId, string token)
        {
            Deck deck = GetDeckById(deckId);

            if (deck is null)
                throw new NotFoundException(DeckMessage.DECK_NOT_FOUND);

            if (deck.Author.Token != token)
            {
                throw new InvalidException(DeckMessage.NOT_AUTHORIZED);
            }

            deckRepository.Delete(deck);
            return true;
        }
    }
}
