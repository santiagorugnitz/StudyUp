namespace Domain
{
    public class UserExam
    {
        public virtual Exam Exam { get; set; }
        public int ExamId { get; set; }
        public double? Score { get; set; }
        public virtual User User { get; set; }
        public int UserId { get; set; }
    }
}
