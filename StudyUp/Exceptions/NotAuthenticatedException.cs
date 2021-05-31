using System;

namespace Exceptions
{
    public class AlreadyExistsException : Exception
    {
        public AlreadyExistsException(string msg) : base(msg) { }
    }
}
