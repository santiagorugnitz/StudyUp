using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exceptions
{
    public static class UserMessage
    {
        public const string USER_NOT_FOUND = "User does not exist.";
        public const string EMAIL_ALREADY_EXISTS = "This email address already belongs to a registered user.";
    }

    public static class DeckMessage
    {
        public const string DECK_ALREADY_EXISTS = "Name already exists.";
        public const string EMPTY_NAME_MESSAGE = "Name field cannot be empty.";
        public const string INVALID_DIFFICULTY = "Choose a valid difficulty.";
        public const string DECK_NOT_FOUND = "This deck does not exist.";
    }
}
