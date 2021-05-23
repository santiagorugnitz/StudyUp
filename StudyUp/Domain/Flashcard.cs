using System.Collections.Generic;

namespace Domain
{
    public class Flashcard
    {
        public int Id { get; set; }
        public string Answer { get; set; }
        public virtual List<FlashcardComment> Comments { get; set; }
        public virtual Deck Deck { get; set; }
        public string Question { get; set; }
        public virtual List<FlashcardScore> UserScores { get; set; }
    }
}
