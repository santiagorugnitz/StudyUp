using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogicInterface;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;
using Domain;
using Exceptions;

namespace WebAPI.Controllers
{
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
            try
            {
                User newUser = logic.AddUser(userModel.ToEntity());
                return Ok(newUser);
            }
            catch (AlreadyExistsException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
