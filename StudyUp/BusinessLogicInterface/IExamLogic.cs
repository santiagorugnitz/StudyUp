using Domain;
using Domain.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogicInterface
{
    public interface IExamLogic
    {
        Exam AddExam(Exam exam, string userToken);
        IEnumerable<Exam> GetTeachersExams(string token);
    }
}