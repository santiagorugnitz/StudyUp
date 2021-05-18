using BusinessLogicInterface;
using Domain;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Filters;
using WebAPI.Models.ResponseModels;

namespace WebAPI.Controllers
{
    [ServiceFilter(typeof(ExceptionFilter))]
    [Route("api/tasks")]
    [ApiController]
    public class TasksController : Controller
    {
        private readonly IUserLogic logic;

        public TasksController(IUserLogic logic)
        {
            this.logic = logic;
        }
        
        [HttpGet]
        public IActionResult GetTasks([FromHeader] string token)
        {
            var tasks = this.logic.GetTasks(token);
            var response = new ResponseTasksModel() { Decks = new List<ResponseTaskDeckModel>(), Exams = new List<ResponseTaskExamModel>() };

            foreach (Deck deck in tasks.Item1)
            {
                response.Decks.Add(new ResponseTaskDeckModel() { Id = deck.Id, Name = deck.Name });
            }

            foreach (Exam exam in tasks.Item2)
            {
                response.Exams.Add(new ResponseTaskExamModel() { Id = exam.Id, Name = exam.Name, GroupsName = exam.Group.Name });
            }

            return Ok(response);
        }
    }
}
