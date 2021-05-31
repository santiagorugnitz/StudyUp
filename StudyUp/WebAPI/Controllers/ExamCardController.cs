using BusinessLogicInterface;
using Domain;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Filters;
using WebAPI.Models.RequestModels;
using WebAPI.Models.ResponseModels;

namespace WebAPI.Controllers
{
    [ServiceFilter(typeof(ExceptionFilter))]
    [Route("api/examcards")]
    [ApiController]
    public class ExamCardController : ControllerBase
    {
        private readonly IExamCardLogic logic;

        public ExamCardController(IExamCardLogic logic)
        {
            this.logic = logic;
        }

        [HttpPost]
        public IActionResult Post([FromBody] ExamCardModel examCardModel, [FromHeader] string token)
        {
            var response = logic.AddExamCard(examCardModel.ExamId, examCardModel.ToEntity(), token);
            return Ok(new ResponseExamCardModel() { Answer = response.Answer, Question = response.Question, Id = response.Id });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id, [FromHeader] string token)
        {
            logic.DeleteExamCard(id, token);
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult EditExamCard([FromRoute] int id, [FromHeader] string token,
           [FromBody] EditExamCardModel editExamCardModel)
        {
            ExamCard newExamCard = new ExamCard()
            {
                Answer = editExamCardModel.Answer,
                Question = editExamCardModel.Question
            };
            logic.EditExamCard(token, id, newExamCard);
            return Ok();
        }
    }
}
