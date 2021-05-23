using System.Collections.Generic;
using WebAPI.Models.ResponseModels;

namespace WebAPI.Models
{
    public class ResponseFlashcardModel
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public IEnumerable<ResponseFlashcardCommentsModel> Comments { get; set; }

    }
}
