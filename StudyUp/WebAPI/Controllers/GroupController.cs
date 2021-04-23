﻿using BusinessLogicInterface;
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

        [HttpGet]
        public IActionResult Get([FromHeader] string token)
        {
            return Ok(ConvertGroups(logic.GetAllGroups(), token));
        }

        private IEnumerable<ResponseGroupModel> ConvertGroups(IEnumerable<Group> groupsList, string token)
        {
            return (groupsList.Select(group => new ResponseGroupModel()
            {
                Id = group.Id,
                Name = group.Name,
                Subscribed = logic.UserIsSubscribed(token, group.Id),
                TeachersName = group.Creator.Username
            }));

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

        [HttpDelete("{id}/unsubscribe")]
        public IActionResult Unsubscribe([FromHeader] string token, [FromRoute] int id)
        {
            return Ok(logic.Unsubscribe(token, id));
        }
    }
}
