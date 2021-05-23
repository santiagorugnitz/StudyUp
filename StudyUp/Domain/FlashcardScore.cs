namespace Domain
{
    public class FlashcardScore
    {
        public virtual Flashcard Flashcard { get; set; }
        public int FlashcardId { get; set; }
        public int Score { get; set; }
        public virtual User User { get; set; }
        public int UserId { get; set; }
    }
}
