using Domain.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class ResponseFullDeckModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Difficulty Difficulty { get; set; }
        public string Subject { get; set; }
        public bool IsHidden { get; set; }
        public virtual string Author { get; set; }
        public IEnumerable<ResponseFlashcardModel> Flashcards { get; set; }
        public string groupName;

    }
}
