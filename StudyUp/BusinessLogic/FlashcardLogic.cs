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
    public class FlashcardLogic : IFlashcardLogic
    {
        private IRepository<Flashcard> flashcardRepository;
        private IRepository<User> userRepository;
        private IRepository<Deck> deckRepository;
        private IUserRepository userTokenRepository;

        public FlashcardLogic(IRepository<Flashcard> repository, IRepository<User> userRepository,
            IUserRepository userTokenRepository, IRepository<Deck> deckRepository)
        {
            this.flashcardRepository = repository;
            this.userRepository = userRepository;
            this.userTokenRepository = userTokenRepository;
            this.deckRepository = deckRepository;
        }

        public Flashcard AddFlashcard(Flashcard flashcard, int deckId, string token)
        {
            if (flashcard.Question is null || flashcard.Answer is null
                || flashcard.Question.Length == 0 || flashcard.Answer.Length == 0)
                throw new InvalidException(FlashcardMessage.EMPTY_QUESTION_OR_ANSWER);

            User userLoggedByToken = userTokenRepository.GetUserByToken(token);
            if (userLoggedByToken == null)
            {
                throw new InvalidException(FlashcardMessage.NOT_AUTHORIZED);
            }

            Deck deck = deckRepository.GetById(deckId);
            if (deck == null)
                throw new NotFoundException(DeckMessage.DECK_NOT_FOUND);

            IEnumerable<User> flashcardsAuthor = userRepository.GetAll().Where(x => x.Id == deck.Author.Id);

            if (userLoggedByToken != null && flashcardsAuthor != null && userLoggedByToken.Id != deck.Author.Id)
                throw new InvalidException(FlashcardMessage.NOT_AUTHORIZED);

            if (userLoggedByToken is null || flashcardsAuthor is null)
                throw new InvalidException(FlashcardMessage.ERROR_ASSOCIATING_USER);

            flashcard.Deck = deck;
            flashcardRepository.Add(flashcard);

            deck.Flashcards.Add(flashcard);
            deckRepository.Update(deck);
            return flashcard;
        }

        public bool DeleteFlashcard(int id, string token)
        {
            Flashcard flashcard = flashcardRepository.GetById(id);

            if (flashcard is null)
                throw new NotFoundException(FlashcardMessage.FLASHCARD_NOT_FOUND);

            User user = this.userRepository.GetById(flashcard.Deck.Author.Id);

            if (!user.Token.Equals(token))
                throw new InvalidException(FlashcardMessage.NOT_AUTHORIZED_TO_DELETE);
            
            Deck deck = flashcard.Deck;
            deck.Flashcards.Remove(flashcard);
            deckRepository.Update(deck);
            userRepository.Update(deck.Author);
            //flashcardRepository.Delete(flashcard);
            return true;
        }

        public Flashcard EditFlashcard(string token, int flashcardId, string newQuestion, string newAnswer)
        {
            Flashcard flashcard = flashcardRepository.GetById(flashcardId);
            User user = userTokenRepository.GetUserByToken(token);

            if (user is null)
                throw new NotFoundException(UserMessage.USER_NOT_FOUND);

            else if (flashcard is null)
                throw new NotFoundException(FlashcardMessage.FLASHCARD_NOT_FOUND);

            else if (user.Id != flashcard.Deck.Author.Id)
                throw new InvalidException(FlashcardMessage.NOT_AUTHORIZED_EDIT);

            else if (flashcard != null && user != null)
            {
                flashcard.Question = newQuestion;
                flashcard.Answer = newAnswer;
                flashcardRepository.Update(flashcard);
            }
            Flashcard updatedFlashcard = flashcardRepository.GetById(flashcardId);
            return updatedFlashcard;
        }
    }
}
