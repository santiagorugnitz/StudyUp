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
using WebAPI.Models.ResponseModels;

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

        [HttpGet]
        public IActionResult GetTeachersExams([FromHeader] string token)
        {
            return Ok(ConvertExams(logic.GetTeachersExams(token)));
        }

        private IEnumerable<ResponseExamGroupModel> ConvertExams(IEnumerable<Exam> examsList)
        {
            List<ResponseExamGroupModel> toReturn = new List<ResponseExamGroupModel>();
            foreach (Exam exam in examsList)
            {
                ResponseExamGroupModel toAdd = new ResponseExamGroupModel();
                toAdd.Name = exam.Name;
                if (exam.Group is null)
                    toAdd.groupsName = " ";
                else
                    toAdd.groupsName = exam.Group.Name;
                toReturn.Add(toAdd);
            }
            return toReturn;
        }

        [HttpGet("{id}")]
        public IActionResult GetExamById([FromRoute] int id, [FromHeader] string token)
        {
            Exam exam = logic.GetExamById(id, token);

            string groupName;
            if (exam.Group is null)
                groupName = " ";
            else
                groupName = exam.Group.Name;

            return Ok(new ResponseFullExamModel()
            {
                Id = exam.Id,
                Name = exam.Name,
                Subject = exam.Subject,
                Difficulty = exam.Difficulty,
                GroupName = groupName,
                Examcards = exam.ExamCards.Select(examcard => new ResponseExamCardModel()
                {
                    Id = examcard.Id,
                    Question = examcard.Question,
                    Answer = examcard.Answer
                })
            });
        }
    }
}