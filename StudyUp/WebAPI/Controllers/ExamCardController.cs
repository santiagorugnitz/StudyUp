using BusinessLogicInterface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Filters;
using WebAPI.Models.RequestModels;

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
            return Ok(logic.AddExamCard(examCardModel.ExamId, examCardModel.ToEntity(), token));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id, [FromHeader] string token)
        {
            logic.DeleteExamCard(id, token);
            return Ok();
        }

    }
}
