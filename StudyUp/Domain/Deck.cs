using Domain.Enumerations;
using System.Collections.Generic;

namespace Domain
{
    public class Deck
    {
        public int Id { get; set; }
        public virtual User Author { get; set; }
        public virtual List<DeckGroup> DeckGroups { get; set; }
        public Difficulty Difficulty { get; set; }
        public virtual List<Flashcard> Flashcards { get; set; }
        public bool IsHidden { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }

        public Deck()
        {
            IsHidden = false;
        }

        public override bool Equals(object o)
        {
            if (o == null || !GetType().Equals(o.GetType()))
            {
                return false;
            }
            else
            {
                Deck anotherDeck = (Deck)o;
                return this.Name.ToString().Equals(anotherDeck.Name.ToString());
            }
        }
    }
}
