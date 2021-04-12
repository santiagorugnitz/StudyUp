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

        public FlashcardLogic(IRepository<Flashcard> repository, IRepository<User> userRepository)
        {
            this.flashcardRepository = repository;
            this.userRepository = userRepository;
        }

        public Flashcard AddFlashcard(Flashcard flashcard, int userId)
        {
            if (flashcard.Question is null || flashcard.Answer is null 
                || flashcard.Question.Length == 0 || flashcard.Answer.Length == 0)
                throw new InvalidException(FlashcardMessage.EMPTY_QUESTION_OR_ANSWER);

            IEnumerable<User> userLogged = userRepository.GetAll().Where(x => x.Id == userId);
            IEnumerable<User> flashcardsAuthor = userRepository.GetAll().Where(x => x.Id == flashcard.Deck.Author.Id);

            if (userLogged != null && flashcardsAuthor != null && userId != flashcard.Deck.Author.Id)
                throw new InvalidException(FlashcardMessage.NOT_AUTHORIZED);

            if (userLogged is null || flashcardsAuthor is null)
                throw new InvalidException(FlashcardMessage.ERROR_ASSOCIATING_USER);

            flashcardRepository.Add(flashcard);

            User user = userRepository.GetById(userId);
            userRepository.Update(user);
            return flashcard;
        }
    }
}
