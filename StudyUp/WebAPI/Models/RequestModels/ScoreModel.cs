using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models.RequestModels
{
    public class ScoreModel
    {
        public int Time { get; set; }
        public int CorrectAnswers { get; set; }
    }
}
