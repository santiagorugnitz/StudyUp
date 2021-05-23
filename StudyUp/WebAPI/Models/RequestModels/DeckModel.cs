using Domain;
using Domain.Enumerations;

namespace WebAPI.Models
{
    public class DeckModel
    {
        public string Name { get; set; }
        public Difficulty Difficulty { get; set; }
        public bool IsHidden { get; set; }
        public string Subject { get; set; }

        public Deck ToEntity() => new Deck()
        {
            Name = this.Name,
            Difficulty = this.Difficulty,
            IsHidden = this.IsHidden,
            Subject = this.Subject
        };
    }
}
