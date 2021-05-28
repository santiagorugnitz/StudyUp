using System;

namespace WebAPI.Models.ResponseModels
{
    public class ResponseFlashcardCommentsModel
    {
        public int Id { get; set; }
        public string AuthorUsername { get; set; }
        public string Comment { get; set; }
        public DateTime Time { get; set; }
    }
}
