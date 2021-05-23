using Domain;

namespace WebAPI.Models
{
    public class FlashcardModel
    {
        public string Answer { get; set; }
        public int DeckId { get; set; }
        public string Question { get; set; }

        public Flashcard ToEntity() => new Flashcard()
        {
           Question = this.Question,
           Answer = this.Answer
        };
    }
}
