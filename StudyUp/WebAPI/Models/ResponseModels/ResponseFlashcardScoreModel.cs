namespace WebAPI.Models
{
    public class ResponseFlashcardScoreModel
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public int Score { get; set; }
    }
}
