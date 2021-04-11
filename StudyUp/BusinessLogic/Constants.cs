using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic
{
    public class Constants
    {
        public const string VALID_PASSWORD = @"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{6,}$";
        public const string VALID_EMAIL = @"^([0-9a-zA-Z]" + @"([\+\-_\.][0-9a-zA-Z]+)*" + @")+" +
                                            @"@(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]*\.)+[a-zA-Z0-9]{2,17})$";
    }
}
