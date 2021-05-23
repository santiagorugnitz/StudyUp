using System.Collections.Generic;

namespace Domain
{
    public class Exam
    {
        public int Id { get; set; }
        public virtual List<UserExam> AlreadyPerformed { get; set; }
        public virtual User Author { get; set; }
        public Enumerations.Difficulty Difficulty { get; set; }
        public virtual List<ExamCard> ExamCards { get; set; }
        public virtual Group Group { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }

        public override bool Equals(object o)
        {
            if (o == null || !GetType().Equals(o.GetType()))
            {
                return false;
            }
            else
            {
                Exam anotherExam = (Exam)o;
                return (this.Name.ToUpper().Equals(anotherExam.Name.ToUpper())
                    && this.Author.Equals(anotherExam.Author));
            }
        }
    }
}
