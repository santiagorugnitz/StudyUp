using System.Collections.Generic;

namespace Domain
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual User Creator { get; set; }
        public virtual List<UserGroup> UserGroups { get; set; }
        public virtual List<DeckGroup> DeckGroups { get; set; }
        public virtual List<Exam> AssignedExams { get; set; }

        public override bool Equals(object o)
        {
            if (o == null || !GetType().Equals(o.GetType()))
            {
                return false;
            }
            else
            {
                Group anotherGroup = (Group)o;
                return this.Creator.Equals(anotherGroup.Creator) && this.Name.ToString().Equals(anotherGroup.Name.ToString());
            }
        } 
    }
}
