using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class FlashcardComment
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedOn { get; set; }
        public virtual Flashcard Flashcard { get; set; }
        public string CreatorUsername { get; set; }
    }
}
