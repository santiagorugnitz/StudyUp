using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class ExamCard
    {
        public int Id { get; set; }
        public virtual Exam Exam { get; set; }
        public string Question { get; set; }
        public bool Answer { get; set; }
    }
}
