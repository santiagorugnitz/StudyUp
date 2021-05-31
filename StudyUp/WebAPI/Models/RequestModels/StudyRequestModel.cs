using System.Collections.Generic;

namespace WebAPI.Models
{
    public class SingleFlashcardModel
    {
        public int FlashcardId { get; set; }
        public int Score { get; set; }
    }

    public class StudyRequestModel
    {
        public List<SingleFlashcardModel> StudyRequest { get; set; }
    }
}
