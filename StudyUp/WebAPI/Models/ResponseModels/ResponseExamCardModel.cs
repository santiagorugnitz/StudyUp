using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;

namespace WebAPI.Models.ResponseModels
{
    public class ResponseExamCardModel
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public bool Answer { get; set; }

        public ExamCard ToEntity() => new ExamCard()
        {
            Question = this.Question,
            Answer = this.Answer
        };
    }
}
