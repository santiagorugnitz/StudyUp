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
        private IUserRepository userTokenRepository;

        public FlashcardLogic(IRepository<Flashcard> repository, IRepository<User> userRepository,
            IUserRepository userTokenRepository)
        {
            this.flashcardRepository = repository;
            this.userRepository = userRepository;
            this.userTokenRepository = userTokenRepository;
        }

        public Flashcard AddFlashcard(Flashcard flashcard, string token)
        {
            if (flashcard.Question is null || flashcard.Answer is null
                || flashcard.Question.Length == 0 || flashcard.Answer.Length == 0)
                throw new InvalidException(FlashcardMessage.EMPTY_QUESTION_OR_ANSWER);

            User userLoggedByToken = userTokenRepository.GetUserByToken(token);

            //IEnumerable<User> userLogged = userRepository.GetAll().Where(x => x.Id == userByToken.Id);
            IEnumerable<User> flashcardsAuthor = userRepository.GetAll().Where(x => x.Id == flashcard.Deck.Author.Id);

            if (userLoggedByToken != null && flashcardsAuthor != null && userLoggedByToken.Id != flashcard.Deck.Author.Id)
                throw new InvalidException(FlashcardMessage.NOT_AUTHORIZED);

            if (userLoggedByToken is null || flashcardsAuthor is null)
                throw new InvalidException(FlashcardMessage.ERROR_ASSOCIATING_USER);

            flashcardRepository.Add(flashcard);

            User user = userRepository.GetById(userLoggedByToken.Id);
            userRepository.Update(user);
            return flashcard;
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
