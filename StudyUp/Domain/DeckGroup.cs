namespace Domain
{
    public class DeckGroup
    {
        public virtual Deck Deck { get; set; }
        public int DeckId { get; set; }
        public int GroupId { get; set; }
        public virtual Group Group { get; set; }
    }
}