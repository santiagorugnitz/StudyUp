namespace Domain
{
    public class UserGroup
    {
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public int GroupId { get; set; }
        public virtual Group Group { get; set; }
    }
}
