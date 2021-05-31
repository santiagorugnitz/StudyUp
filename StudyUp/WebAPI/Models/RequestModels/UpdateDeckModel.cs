using Domain.Enumerations;

namespace WebAPI.Models
{
    public class UpdateDeckModel
    {
        public Difficulty Difficulty { get; set; }
        public bool IsHidden { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }
    }
}
