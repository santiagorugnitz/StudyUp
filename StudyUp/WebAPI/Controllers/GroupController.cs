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
        public IActionResult Post([FromBody] GroupModel groupModel, [FromHeader] string token)
        {
            return Ok(logic.AddGroup(groupModel.ToEntity(), token));
        }

        [HttpPost("{id}/subscribe")]
        public IActionResult Subscribe([FromHeader] string token, [FromRoute] int id)
        {
            return Ok(logic.Subscribe(token, id));
        }
    }
}
