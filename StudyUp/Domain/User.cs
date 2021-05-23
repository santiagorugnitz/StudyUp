using System.Collections.Generic;

namespace Domain
{
    public class User
    {
        public int Id { get; set; }
        public virtual List<Deck> Decks { get; set; }
        public string Email { get; set; }
        public virtual List<Exam> Exams { get; set; }
        public virtual List<FlashcardScore> FlashcardScores { get; set; }
        public string FirebaseToken { get; set; }
        public virtual List<UserFollowing> FollowingUsers { get; set; }
        public virtual List<UserFollowing> FollowedUsers { get; set; }
        public virtual List<Group> Groups { get; set; }
        public bool IsStudent { get; set; }
        public string Password { get; set; }
        public virtual List<UserExam> SolvedExams { get; set; }
        public string Token { get; set; }
        public virtual List<UserGroup> UserGroups { get; set; }
        public string Username { get; set; }

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
