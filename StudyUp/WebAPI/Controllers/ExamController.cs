using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using BusinessLogicInterface;
using Domain;
using WebAPI.Models;
using WebAPI.Filters;
using WebAPI.Models.ResponseModels;
using WebAPI.Models.RequestModels;

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

        [HttpPost("{examId}/assign")]
        public IActionResult Assign([FromHeader] string token, [FromRoute] int examId,
            [FromQuery] int groupId)
        {
            return Ok(logic.AssignExam(token, groupId, examId));
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

        [HttpPost("{examId}/results")]
        public IActionResult AssignResults([FromRoute] int examId, [FromHeader] string token, [FromBody] ScoreModel score)
        {
            logic.AssignResults(examId, token, score.Time, score.CorrectAnswers);
            return Ok();
        }

        [HttpGet("{examId}/results")]
        public IActionResult GetResults([FromRoute] int examId, [FromHeader] string token)
        {
            var list = logic.GetResults(examId, token);
            var response = new List<ResponseResultModel>();
            foreach (var item in list)
            {
                response.Add(new ResponseResultModel() { Username = item.Item1, Score = item.Item2 });
            }
            return Ok(response);
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
                toAdd.Id = exam.Id;
                toAdd.Name = exam.Name;
                if (exam.Group is null)
                    toAdd.GroupsName = "";
                else
                    toAdd.GroupsName = exam.Group.Name;
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
                groupName = "";
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