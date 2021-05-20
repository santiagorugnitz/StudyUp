using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exceptions
{
    public static class CommentMessage
    {
        public const string COMMENT_NOT_FOUND = "This comment does not exist.";
        public const string NOT_AUTHORIZED = "Only this flashcards author can delete its comments.";
    }

    public static class DeckMessage
    {
        public const string DECK_ALREADY_EXISTS = "Name already exists.";
        public const string EMPTY_NAME = "Name field cannot be empty.";
        public const string INVALID_DIFFICULTY = "Choose a valid difficulty.";
        public const string DECK_NOT_FOUND = "This deck does not exist.";
        public const string EMPTY_SUBJECT = "Subject field cannot be empty.";
        public const string NOT_AUTHORIZED = "Only this decks author can delete it.";
        public const string NOT_ASSIGNED = "This deck is not assigned to this group.";
        public const string ALREADY_ASSIGNED = "Already assigned this deck to this group.";
    }

    public static class ExamCardMessage
    {
        public const string INVALID_AUTHOR = "User is not this exams author.";
        public const string EXAMCARD_ALREADY_EXISTS = "Question already exists.";
        public const string EXAMCARD_NOT_FOUND = "This examcard does not exist.";
        public const string NOT_AUTHORIZED_TO_DELETE = "Only the author can delete it.";
        public const string NOT_AUTHORIZED_TO_EDIT = "Only the author can edit it.";
    }

    public static class ExamMessage
    {
        public const string EXAM_ALREADY_EXISTS = "Name already exists.";
        public const string EMPTY_NAME_MESSAGE = "Exams name field cannot be empty.";
        public const string INVALID_DIFFICULTY = "Choose a valid difficulty for this exam.";
        public const string EMPTY_SUBJECT_MESSAGE = "Subject field cannot be empty.";
        public const string NOT_A_TEACHER = "Only teachers can create and/or edit exams.";
        public const string CANNOT_GET_EXAMS = "This user is not a teacher.";
        public const string EXAM_NOT_FOUND = "This exam does not exist.";
        public const string INVALID_USER = "Logged user and exam author are different.";
        public const string ALREADY_ASSIGNED = "Already assigned this exam to a group.";
        public const string NOT_AUTHORIZED = "Only this groups teacher can assign .";
        public const string ALREADY_COMPLEATED = "This exam has been already compleated";
    }

    public static class FlashcardMessage
    {
        public const string EMPTY_QUESTION_OR_ANSWER = "Question and answer fields cannot be empty.";
        public const string NOT_AUTHORIZED = "Only this decks author can add flashcards to it.";
        public const string ERROR_ASSOCIATING_USER = "User must be logged and must be this decks author.";
        public const string FLASHCARD_NOT_FOUND = "This flashcard does not exist.";
        public const string NOT_AUTHORIZED_EDIT = "Only this flashcards author can edit it.";
        public const string NOT_AUTHORIZED_TO_DELETE = "Only this flashcards author can delete it.";
        public const string FLASHCARDS_AUTHOR_CANNOT_COMMENT_HIS_FLASHCARD = "The author of the flashcards cannot comment it";
        public const string LARGE_COMMENT = "The comment is larger than 180 characters";
    }

    public static class GroupMessage
    {
        public const string GROUP_ALREADY_EXISTS = "Name already exists.";
        public const string EMPTY_NAME_MESSAGE = "Name field cannot be empty.";
        public const string GROUP_NOT_FOUND = "This group does not exist.";
        public const string ALREADY_SUBSCRIBED = "Already subscribed to this group.";
        public const string NOT_SUBSCRIBED = "This subscription does not exist.";
        public const string NOT_AUTHORIZED = "Only this groups teacher can assign decks to study.";
        public const string NO_DECKS = "There are no decks assigned to this group.";
    }

    public static class UserMessage
    {
        public const string INVALID_PASSWORD = "Password must be 6 characters long and contain at least" +
            " one number, an uppercase letter and a lowercase letter.";
        public const string USER_NOT_FOUND = "User does not exist.";
        public const string EMAIL_ALREADY_EXISTS = "This email address already belongs to a registered user.";
        public const string USERNAME_ALREADY_EXISTS = "This username already belongs to a registered user.";
        public const string WRONG_EMAIL_OR_PASSWORD = "Wrong email or password.";
        public const string WRONG_USERNAME_OR_PASSWORD = "Wrong username or password.";
        public const string INVALID_EMAIL = "Invalid mail address.";
        public const string ALREADY_FOLLOWS = "You already follow this user.";
        public const string NOT_FOLLOWS = "You cannot unfollow an unfollowed user.";
    }

    public static class UnauthenticatedMessage
    {
        public const string UNAUTHENTICATED_USER = "The user is not authenticated.";
    }
}
