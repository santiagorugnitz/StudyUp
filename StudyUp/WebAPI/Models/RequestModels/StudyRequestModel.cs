using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class SingleFlashcardModel
    {
        public int Score { get; set; }
        public int FlashcardId { get; set; }
    }

    public class StudyRequestModel
    {
        public List<SingleFlashcardModel> StudyRequest { get; set; }
    }
}
