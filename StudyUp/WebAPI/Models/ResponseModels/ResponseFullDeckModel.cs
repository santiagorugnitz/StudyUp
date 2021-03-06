using Domain.Enumerations;
using System.Collections.Generic;

namespace WebAPI.Models
{
    public class ResponseFullDeckModel
    {
        public int Id { get; set; }
        public virtual string Author { get; set; }
        public Difficulty Difficulty { get; set; }
        public IEnumerable<ResponseFlashcardModel> Flashcards { get; set; }
        public string GroupName;
        public bool IsHidden { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }

    }
}
