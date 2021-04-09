using System;
using System.Collections.Generic;

namespace Domain
{
    public class User
    {
        public int Id { get; set; }
        public virtual List<Deck> Decks { get; set; }
        public string Email { get; set; }
        public bool IsATeacher { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }

        public override bool Equals(object o)
        {
            if (o == null || !GetType().Equals(o.GetType()))
            {
                return false;
            }
            else
            {
                User anotherUser = (User)o;
                return this.Email.ToString().Equals(anotherUser.Email.ToString());
            }
        }
    }
}
