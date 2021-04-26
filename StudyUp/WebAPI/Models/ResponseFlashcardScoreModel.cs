using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
