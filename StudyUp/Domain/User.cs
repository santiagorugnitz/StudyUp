using System;
using System.Collections.Generic;

namespace Domain
{
    public class User
    {
        public int Id { get; set; }
        public virtual List<Deck> Decks { get; set; }
        public virtual List<Group> Groups { get; set; }
        public virtual List<UserGroup> UserGroups { get; set; }
        public virtual List<FlashcardScore> FlashcardScores { get; set; }
        public string Email { get; set; }
        public bool IsStudent { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public int? FollowerId { get; set; }
        public virtual User Follower { get; set; }
        public virtual List<User> FollowedUsers { get; set; }

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
