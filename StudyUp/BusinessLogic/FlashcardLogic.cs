using BusinessLogicInterface;
using DataAccessInterface;
using Domain;
using Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogic
{
    public class FlashcardLogic : IFlashcardLogic
    {
        private IRepository<Flashcard> flashcardRepository;
        private IRepository<User> userRepository;
        private IRepository<Deck> deckRepository;
        private IRepository<FlashcardScore> flashcardScoreRepository;
        private IRepository<FlashcardComment> flashcardCommentRepository;
        private IUserRepository userTokenRepository;
        private INotifications notificationsInterface;

        public FlashcardLogic(IRepository<Flashcard> repository, IRepository<User> userRepository,
            IUserRepository userTokenRepository, IRepository<Deck> deckRepository,
            IRepository<FlashcardScore> flashcardScoreRepository,
            IRepository<FlashcardComment> flashcardCommentRepository,
            INotifications notificationsInterface)
        {
            this.flashcardRepository = repository;
            this.userRepository = userRepository;
            this.userTokenRepository = userTokenRepository;
            this.deckRepository = deckRepository;
            this.flashcardScoreRepository = flashcardScoreRepository;
            this.flashcardCommentRepository = flashcardCommentRepository;
            this.notificationsInterface = notificationsInterface;
        }

        public Flashcard AddFlashcard(Flashcard flashcard, int deckId, string token)
        {
            if (flashcard.Question is null || flashcard.Answer is null
                || flashcard.Question.Length == 0 || flashcard.Answer.Length == 0)
                throw new InvalidException(FlashcardMessage.EMPTY_QUESTION_OR_ANSWER);

            User userLoggedByToken = UserByToken(token);

            Deck deck = deckRepository.GetById(deckId);
            if (deck == null)
                throw new NotFoundException(DeckMessage.DECK_NOT_FOUND);

            if (userLoggedByToken.Id != deck.Author.Id)
                throw new InvalidException(FlashcardMessage.NOT_AUTHORIZED);

            flashcard.Deck = deck;
            flashcardRepository.Add(flashcard);

            deck.Flashcards.Add(flashcard);
            deckRepository.Update(deck);
            return flashcard;
        }

        public void CommentFlashcard(int flashcardId, string token, string comment)
        {
            Flashcard flashcard = flashcardRepository.GetById(flashcardId);

            if (flashcard is null)
                throw new NotFoundException(FlashcardMessage.FLASHCARD_NOT_FOUND);

            User user = userTokenRepository.GetUserByToken(token);

            if (user is null)
                throw new NotFoundException(UserMessage.USER_NOT_FOUND);

            if (flashcard.Deck != null && flashcard.Deck.Author == user)
                throw new InvalidException(FlashcardMessage.FLASHCARDS_AUTHOR_CANNOT_COMMENT_THEIR_FLASHCARD);

            if (comment.Length > 180)
                throw new InvalidException(FlashcardMessage.LARGE_COMMENT);

            var commentModel = new FlashcardComment()
            {
                Comment = comment,
                Flashcard = flashcard,
                CreatedOn = DateTime.Now,
                CreatorUsername = user.Username
            };

            flashcardCommentRepository.Add(commentModel);

            this.notificationsInterface.NotifyComments(commentModel, flashcard.Deck.Author);

            flashcard.Comments.Add(commentModel);
            flashcardRepository.Update(flashcard);
        }

        public bool DeleteComment(string token, int flashcardId, int commentId)
        {
            User user = UserByToken(token);
            Flashcard flashcard = flashcardRepository.GetById(flashcardId);
            FlashcardComment comment = flashcardCommentRepository.GetById(commentId);

            if (flashcard is null)
                throw new NotFoundException(FlashcardMessage.FLASHCARD_NOT_FOUND);

            if (!flashcard.Deck.Author.Equals(user))
                throw new InvalidException(CommentMessage.NOT_AUTHORIZED);

            if (comment is null)
                throw new NotFoundException(CommentMessage.COMMENT_NOT_FOUND);

            flashcard.Comments.Remove(comment);
            flashcardRepository.Update(flashcard);
            return true;
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
            return true;
        }

        public Flashcard EditFlashcard(string token, int flashcardId, string newQuestion,
            string newAnswer)
        {
            Flashcard flashcard = flashcardRepository.GetById(flashcardId);
            User user = UserByToken(token);

            if (flashcard is null)
                throw new NotFoundException(FlashcardMessage.FLASHCARD_NOT_FOUND);

            else if (user.Id != flashcard.Deck.Author.Id)
                throw new InvalidException(FlashcardMessage.NOT_AUTHORIZED_TO_EDIT);

            else if (flashcard != null && user != null)
            {
                flashcard.Question = newQuestion;
                flashcard.Answer = newAnswer;
                flashcardRepository.Update(flashcard);
            }
            Flashcard updatedFlashcard = flashcardRepository.GetById(flashcardId);
            return updatedFlashcard;
        }

        public List<Tuple<Flashcard, int>> GetRatedFlashcards(int deckId, string token)
        {
            Deck deck = deckRepository.GetById(deckId);
            if (deck == null)
                throw new NotFoundException(DeckMessage.DECK_NOT_FOUND);

            User user = UserByToken(token);

            List<Tuple<Flashcard, int>> returningList = new List<Tuple<Flashcard, int>>();

            foreach (var flashcard in deck.Flashcards)
            {
                var flashcardScore = flashcardScoreRepository.FindByCondition(fs => fs.FlashcardId == flashcard.Id
                && fs.UserId == user.Id);
                if (flashcardScore.Count() == 0)
                {
                    Tuple<Flashcard, int> assigningTuple = new Tuple<Flashcard, int>(flashcard, 0);
                    returningList.Add(assigningTuple);
                }
                else
                {
                    Tuple<Flashcard, int> assigningTuple = new Tuple<Flashcard, int>(flashcard,
                        flashcardScore.First().Score);
                    returningList.Add(assigningTuple);
                }
            }
            return returningList;
        }

        public Flashcard UpdateScore(int id, int score, string token)
        {
            Flashcard flashcard = flashcardRepository.GetById(id);

            if (flashcard is null)
                throw new NotFoundException(FlashcardMessage.FLASHCARD_NOT_FOUND);

            User user = UserByToken(token);

            var flashcardScore = flashcardScoreRepository.FindByCondition(fs => fs.FlashcardId == flashcard.Id && fs.UserId == user.Id);

            if (flashcardScore.Count() == 0)
            {
                var addingFlashcard = new FlashcardScore()
                {
                    FlashcardId = flashcard.Id,
                    Flashcard = flashcard,
                    User = user,
                    UserId = user.Id,
                    Score = score
                };

                flashcardScoreRepository.Add(addingFlashcard);

                flashcard.UserScores.Add(addingFlashcard);
                flashcardRepository.Update(flashcard);
            }
            else
            {
                var editingFlashcard = flashcardScore.First();
                editingFlashcard.Score = score;

                flashcard.UserScores.Find(fs => fs.UserId == user.Id).Score = score;
                flashcardRepository.Update(flashcard);

                flashcardScoreRepository.Update(editingFlashcard);
            }
            return flashcard;
        }

        private User UserByToken(string token)
        {
            User user = userTokenRepository.GetUserByToken(token);

            if (user is null)
                throw new InvalidException(UnauthenticatedMessage.UNAUTHENTICATED_USER);
            else
                return user;
        }
    }
}
