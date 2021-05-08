using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class UserExam
    {
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public int ExamId { get; set; }
        public virtual Exam Exam { get; set; }
        public int? Score { get; set; }
    }
}
