using BusinessLogicInterface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Filters;

namespace WebAPI
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
            User newUser = logic.AddUser(userModel.ToEntity());
            ResponseUserModel responseUserModel = new ResponseUserModel()
            {
                Id = newUser.Id,
                Email = newUser.Email,
                IsStudent = newUser.IsStudent,
                Username = newUser.Username,
                Token = newUser.Token
            };
            return Ok(responseUserModel);
        }
    }
}
