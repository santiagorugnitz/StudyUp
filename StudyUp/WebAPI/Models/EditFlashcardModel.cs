using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class EditFlashcardModel
    {
        public int Id; //flashcard to modify
        public string Answer;
        public string Question;
    }
}
