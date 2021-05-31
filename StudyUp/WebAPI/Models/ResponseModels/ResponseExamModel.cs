using Domain.Enumerations;

namespace WebAPI.Models
{
    public class ResponseExamModel
    {
        public int Id { get; set; }
        public Difficulty Difficulty { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }
    }
}
