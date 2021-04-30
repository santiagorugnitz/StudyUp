using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class DeckGroup
    {
        public int GroupId { get; set; }
        public virtual Group Group { get; set; }
        public int DeckId { get; set; }
        public virtual Deck Deck { get; set; }
    }
}