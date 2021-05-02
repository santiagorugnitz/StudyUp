using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Domain.Enumerations;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Models
{
    public class ExamModel
    {
        public string Name { get; set; }
        public Difficulty Difficulty { get; set; }
        public string Subject { get; set; }

        public Exam ToEntity() => new Exam()
        {
            Name = this.Name,
            Difficulty = this.Difficulty,
            Subject = this.Subject
        };
    }
}