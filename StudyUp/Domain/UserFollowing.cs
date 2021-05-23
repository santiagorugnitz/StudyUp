using System;

namespace Domain
{
    public class UserFollowing
    {
        public virtual User FollowingUser { get; set; }
        public int FollowingUserId { get; set; }
        public virtual User FollowerUser { get; set; }
        public int FollowerUserId { get; set; }

        public override bool Equals(object o)
        {
            if (o == null)
            {
                return false;
            }
            else
            {
                try
                {
                    UserFollowing anotherUserFollowing = (UserFollowing)o;
                    return this.FollowingUserId == anotherUserFollowing.FollowingUserId &&
                        this.FollowerUserId == anotherUserFollowing.FollowerUserId;
                } 
                catch (Exception ex)
                {
                    return false;
                }
            }
        }
    }
}
