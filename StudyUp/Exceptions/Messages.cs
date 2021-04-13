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
    }

    public static class GroupMessage
    {
        public const string GROUP_ALREADY_EXISTS = "Name already exists.";
        public const string EMPTY_NAME_MESSAGE = "Name field cannot be empty.";
    }
}
