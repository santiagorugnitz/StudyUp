using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class Exam
    {
        public int Id { get; set; }
        public virtual User Author { get; set; }
        public string Name { get; set; }
        public Enumerations.Difficulty Difficulty { get; set; }
        public string Subject { get; set; }
        public virtual List<ExamCard> ExamCards { get; set; }
        public virtual Group Group { get; set; }
    }
}
