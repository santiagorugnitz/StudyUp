using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace BusinessLogic
{
    public class ValidationService
    {
        public bool PasswordValidation(string field)
        {
            return (Regex.IsMatch(field, Constants.VALID_PASSWORD) && field.Length > 5);
        }

        public bool EmailValidation(string field)
        {
            return (Regex.IsMatch(field, Constants.VALID_EMAIL) && field.Length > 0);
        }
    }
}
