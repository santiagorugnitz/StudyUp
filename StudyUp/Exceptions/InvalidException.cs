using System;

namespace Exceptions
{
    public class InvalidException : Exception
    {
        public InvalidException(string msg) : base(msg) { }
    }
}
