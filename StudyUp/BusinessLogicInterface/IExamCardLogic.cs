using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogicInterface
{
    public interface IExamCardLogic
    {
        ExamCard AddExamCard(int examId, ExamCard examCard, string token);
        ExamCard EditExamCard(string token, int examCardId, string newQuestion, bool newAnswer);
        bool DeleteExamCard(int id, string token);
    }
}
