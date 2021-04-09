using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogicInterface;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;
using Domain;
using Exceptions;
using WebAPI.Filters;

namespace WebAPI.Controllers
{
    [ServiceFilter(typeof(ExceptionFilter))]
    [Route("api/users")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserLogic logic;

        public UserController(IUserLogic logic)
        {
            this.logic = logic;
        }

        [HttpPost]
        public IActionResult Post([FromBody] UserModel userModel)
        {
            User newUser = logic.AddUser(userModel.ToEntity());
            ResponseUserModel responseUserModel = new ResponseUserModel() {
                Id = newUser.Id,
                Email = newUser.Email,
                IsStudent = newUser.IsStudent,
                Username = newUser.Username,
                Token = newUser.Token
            };
            return Ok(responseUserModel);
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel loginModel)
        {
            var user = logic.Login(loginModel.Email, loginModel.Password);
            ResponseUserModel responseUserModel = new ResponseUserModel()
            {
                Id = user.Id,
                Email = user.Email,
                IsStudent = user.IsStudent,
                Username = user.Username,
                Token = user.Token
            };
            return Ok(responseUserModel);
        }
    }
}
