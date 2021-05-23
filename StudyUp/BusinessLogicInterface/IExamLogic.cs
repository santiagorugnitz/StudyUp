using Domain;
using System;
using System.Collections.Generic;

namespace BusinessLogicInterface
{
    public interface IExamLogic
    {
        Exam AddExam(Exam exam, string userToken);
        Exam AssignExam(string token, int groupId, int examId);
        IEnumerable<Exam> GetTeachersExams(string token);
        Exam GetExamById(int id, string token);
        List<Tuple<string, double>> GetResults(int examId, string token);
        void AssignResults(int examId, string token, int time, int correctAnswers);
    }
}