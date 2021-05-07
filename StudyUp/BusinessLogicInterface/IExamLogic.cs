﻿using Domain;
using Domain.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogicInterface
{
    public interface IExamLogic
    {
        Exam AddExam(Exam exam, string userToken);
        Exam AssignExam(string token, int groupId, int examId);
        IEnumerable<Exam> GetTeachersExams(string token);
        Exam GetExamById(int id, string token);
    }
}