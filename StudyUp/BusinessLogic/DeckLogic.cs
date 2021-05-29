using BusinessLogicInterface;
using DataAccessInterface;
using Domain;
using Domain.Enumerations;
using Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogic
{
    public class DeckLogic : IDeckLogic
    {
        private IRepository<Deck> deckRepository;
        private IRepository<User> userRepository;
        private IRepository<Group> groupRepository;
        private IRepository<Flashcard> flashcardRepository;
        private IRepository<FlashcardComment> flashcardCommentRepository;
        private IRepository<DeckGroup> deckGroupRepository;
        private IUserRepository userTokenRepository;
        private INotifications notificationsInterface;

        public DeckLogic(IRepository<Deck> repository, IRepository<User> userRepository,
            IUserRepository userTokenRepository, IRepository<Flashcard> flashcardRepository,
            IRepository<DeckGroup> deckGroupRepository, IRepository<Group> groupRepository,
            INotifications notificationsInterface,
            IRepository<FlashcardComment> flashcardCommentRepository)
        {
            this.deckRepository = repository;
            this.userRepository = userRepository;
            this.userTokenRepository = userTokenRepository;
            this.flashcardRepository = flashcardRepository;
            this.deckGroupRepository = deckGroupRepository;
            this.groupRepository = groupRepository;
            this.notificationsInterface = notificationsInterface;
            this.flashcardCommentRepository = flashcardCommentRepository;
        }

        public Deck AddDeck(Deck deck, string userToken)
        {
            IEnumerable<Deck> sameName = deckRepository.GetAll().Where(x => x.Name.Equals(deck.Name));
            if (sameName != null && (sameName.Count() > 0))
                throw new AlreadyExistsException(DeckMessage.DECK_ALREADY_EXISTS);
            else if (deck.Name is null)
                throw new InvalidException(DeckMessage.EMPTY_NAME);
            else if ((int)deck.Difficulty < 0 || (int)deck.Difficulty > 2)
                throw new InvalidException(DeckMessage.INVALID_DIFFICULTY);
            else if (deck.Subject is null)
                throw new InvalidException(DeckMessage.EMPTY_SUBJECT);

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


        public Deck EditDeck(int deckId, string newName, Difficulty newDifficulty, bool newVisibility,
            string subject)
        {
            Deck deck = deckRepository.GetById(deckId);
            ICollection<Deck> sameName = deckRepository.FindByCondition(a => a.Name == newName
                && a.Id != deckId);

            if (sameName != null && sameName.Count > 0)
                throw new AlreadyExistsException(DeckMessage.DECK_ALREADY_EXISTS);
            if ((int)newDifficulty > 2 || (int)newDifficulty < 0)
                throw new InvalidException(DeckMessage.INVALID_DIFFICULTY);
            if (subject is null || subject.Trim().Length == 0)
                throw new InvalidException(DeckMessage.EMPTY_SUBJECT);

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

            if (deck.Author.Token != token)
            {
                throw new InvalidException(DeckMessage.NOT_AUTHORIZED);
            }

            deckRepository.Delete(deck);
            return true;
        }

        public Group Assign(string token, int groupId, int deckId)
        {
            User user = UserByToken(token);
            Deck deck = deckRepository.GetById(deckId);
            Group group = groupRepository.GetById(groupId);

            if (group is null)
                throw new NotFoundException(GroupMessage.GROUP_NOT_FOUND);

            if (deck is null)
                throw new NotFoundException(DeckMessage.DECK_NOT_FOUND);

            if (!group.Creator.Equals(user))
                throw new InvalidException(GroupMessage.NOT_AUTHORIZED);

            var resultFind = deckGroupRepository.FindByCondition(t => t.GroupId == groupId
                    && t.DeckId == deckId);

            if (resultFind.Count > 0)
                throw new AlreadyExistsException(DeckMessage.ALREADY_ASSIGNED);

            DeckGroup deckGroup = new DeckGroup()
            {
                Deck = deck,
                DeckId = deckId,
                Group = group,
                GroupId = groupId
            };

            this.notificationsInterface.NotifyMaterial(deck, group);

            group.DeckGroups.Add(deckGroup);
            groupRepository.Update(group);
            return group;
        }

        public Group Unassign(string token, int groupId, int deckId)
        {
            User user = UserByToken(token);
            Group group = groupRepository.GetById(groupId);

            if (group is null)
                throw new NotFoundException(GroupMessage.GROUP_NOT_FOUND);

            if (!group.Creator.Equals(user))
                throw new InvalidException(GroupMessage.NOT_AUTHORIZED);

            var resultFind = deckGroupRepository.FindByCondition(t => t.GroupId == groupId
                    && t.DeckId == deckId);

            if (resultFind.Count == 0)
                throw new NotFoundException(DeckMessage.NOT_ASSIGNED);

            deckGroupRepository.Delete(resultFind.First());
            group.DeckGroups.Remove(resultFind.First());
            groupRepository.Update(group);
            return group;
        }

        public IEnumerable<FlashcardComment> GetFlashcardsComments(int flashcardId)
        {
            Flashcard flashcard = flashcardRepository.GetById(flashcardId);

            if (flashcard is null)
                throw new NotFoundException(FlashcardMessage.FLASHCARD_NOT_FOUND);

            IEnumerable<FlashcardComment> flashcardsComments = new List<FlashcardComment>();
            flashcardsComments = this.flashcardCommentRepository.FindByCondition(
                c => c.Flashcard.Id == flashcardId);

            return flashcardsComments;
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
