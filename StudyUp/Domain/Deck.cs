using Domain.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class Deck
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Difficulty Difficulty { get; set; }
        public string Subject { get; set; }
        public virtual List<Flashcard> Flashcards { get; set; }

        public bool IsHidden { get; set; }
        public User Author { get; set; }

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
