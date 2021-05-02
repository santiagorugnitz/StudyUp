using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BusinessLogicInterface;
using Exceptions;
using Domain;
using WebAPI.Models;
using WebAPI.Filters;

namespace WebAPI.Controllers
{
    [ServiceFilter(typeof(ExceptionFilter))]
    [Route("api/exams")]
    [ApiController]
    public class ExamController : ControllerBase
    {
        private readonly IExamLogic logic;

        public ExamController(IExamLogic logic)
        {
            this.logic = logic;
        }

        [HttpPost]
        public IActionResult Post([FromBody] ExamModel examModel, [FromHeader] string token)
        {
            Exam exam = logic.AddExam(examModel.ToEntity(), token);
            return Ok(new ResponseExamModel()
            {
                Id = exam.Id,
                Difficulty = exam.Difficulty,
                Name = exam.Name,
                Subject = exam.Subject
            });
        }
    }
}