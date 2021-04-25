using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class ResponseFlashcardScoreModel
    {
        public Flashcard Flashcard { get; set; }
        public int Score { get; set; }
    }
}
