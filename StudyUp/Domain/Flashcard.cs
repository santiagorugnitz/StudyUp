using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class Flashcard
    {
        public int Id { get; set; }
        public virtual Deck Deck { get; set; }
        public virtual List<FlashcardScore> UserScores { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
    }
}
