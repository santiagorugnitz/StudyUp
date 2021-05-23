using Domain;
using Domain.Enumerations;

namespace WebAPI.Models
{
    public class ExamModel
    {
        public Difficulty Difficulty { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }

        public Exam ToEntity() => new Exam()
        {
            Name = this.Name,
            Difficulty = this.Difficulty,
            Subject = this.Subject
        };
    }
}