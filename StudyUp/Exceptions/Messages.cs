using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exceptions
{
    public static class UserMessage
    {
        public const string INVALID_PASSWORD = "Password must be 6 characters long and contain at least" +
            " one number, an uppercase letter and a lowercase letter.";
        public const string USER_NOT_FOUND = "User does not exist.";
        public const string EMAIL_ALREADY_EXISTS = "This email address already belongs to a registered user.";
        public const string WRONG_EMAIL_OR_PASSWORD = "Wrong email or password.";
        public const string WRONG_USERNAME_OR_PASSWORD = "Wrong username or password.";
        public const string INVALID_EMAIL = "Invalid mail address.";
    }

    public static class DeckMessage
    {
        public const string DECK_ALREADY_EXISTS = "Name already exists.";
        public const string EMPTY_NAME_MESSAGE = "Name field cannot be empty.";
        public const string INVALID_DIFFICULTY = "Choose a valid difficulty.";
        public const string DECK_NOT_FOUND = "This deck does not exist.";
        public const string INVALID_DIFFICULTY_MESSAGE = "Invalid difficulty";
        public const string EMPTY_SUBJECT_MESSAGE = "Subject field cannot be empty.";
        public const string NOT_AUTHORIZED = "Only this decks author can delete it.";

    }

    public static class UnauthenticatedMessage
    {
        public const string UNAUTHENTICATED_USER = "The user is not authenticated.";

    }

    public static class FlashcardMessage
    {
        public const string EMPTY_QUESTION_OR_ANSWER = "Question and answer fields cannot be empty.";
        public const string NOT_AUTHORIZED = "Only this decks author can add flashcards to it.";
        public const string ERROR_ASSOCIATING_USER = "User must be logged and must be this decks author.";
        public const string FLASHCARD_NOT_FOUND = "This flashcard does not exist.";
        public const string NOT_AUTHORIZED_EDIT = "Only this flashcards author can edit it.";
        public const string NOT_AUTHORIZED_TO_DELETE = "Only this flashcards author can delete it.";
    }

    public static class GroupMessage
    {
        public const string GROUP_ALREADY_EXISTS = "Name already exists.";
        public const string EMPTY_NAME_MESSAGE = "Name field cannot be empty.";
        public const string GROUP_NOT_FOUND = "This group does not exist.";
        public const string ALREADY_SUBSCRIBED = "Already subscribed to this group.";
        public const string NOT_SUBSCRIBED = "This subscription does not exist.";
        public const string NO_MATCHES = "No group matches this keyword.";
    }
}
