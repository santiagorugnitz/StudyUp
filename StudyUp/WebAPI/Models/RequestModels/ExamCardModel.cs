﻿using Domain;

namespace WebAPI.Models.RequestModels
{
    public class ExamCardModel
    {
        public int ExamId { get; set; }
        public string Question { get; set; }
        public bool Answer { get; set; }

        public ExamCard ToEntity() => new ExamCard()
        {
            Answer = this.Answer,
            Question = this.Question
        };
    }
}
