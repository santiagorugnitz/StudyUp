using BusinessLogicInterface;
using Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Filters;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [ServiceFilter(typeof(ExceptionFilter))]
    [Route("api/groups")]
    [ApiController]
    public class GroupController : Controller
    {
        private readonly IGroupLogic logic;

        public GroupController(IGroupLogic logic)
        {
            this.logic = logic;
        }

        [HttpPost]
        public IActionResult Post([FromBody] GroupModel groupModel, [FromHeader] string creatorsToken)
        {
            Group newGroup = logic.AddGroup(groupModel.ToEntity(), creatorsToken);
            /* TODO: Check if response is going to be a JSON */ 
            return Ok("The model was created successfully");
        }
    }
}
