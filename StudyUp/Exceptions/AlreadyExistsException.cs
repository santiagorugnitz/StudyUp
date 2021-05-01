using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exceptions
{
    public class NotAuthenticatedException : Exception
    {
        public NotAuthenticatedException(string msg) : base(msg) { }
    }
}
