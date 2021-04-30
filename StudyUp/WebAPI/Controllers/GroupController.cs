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

        [HttpGet]
        public IActionResult Get([FromHeader] string token, [FromQuery] string name)
        {
            return Ok(ConvertGroups(logic.GetAllGroups(name), token).OrderBy(a => a.Name.Length));
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

        [HttpPost("{groupId}/assign")]
        public IActionResult Assign([FromHeader] string token, [FromRoute] int groupId,
            [FromQuery] int deckId)
        {
            return Ok(logic.Assign(token, groupId, deckId));
        }

        [HttpDelete("{groupId}/unassign")]
        public IActionResult Unassign([FromHeader] string token, [FromRoute] int groupId,
            [FromQuery] int deckId)
        {
            return Ok(logic.Unassign(token, groupId, deckId));
        }

        [HttpGet]
        public IActionResult Get([FromHeader] string token)
        {
            return Ok(ConvertTeachersGroups(logic.GetTeachersGroups(token)));
        }

        private IEnumerable<ResponseTeachersGroupModel> ConvertTeachersGroups(IEnumerable<Group> groupsList)
        {
            List<ResponseTeachersGroupModel> toReturn = new List<ResponseTeachersGroupModel>();

            foreach (Group group in groupsList)
            {
                ResponseTeachersGroupModel toAdd = new ResponseTeachersGroupModel();
                toAdd.Name = group.Name;

                var fullDeckList = logic.GetGroupDecks(group.Id);
                foreach (Deck deck in fullDeckList)
                {
                    ResponseDeckIdNameModel deckIdNameModel = new ResponseDeckIdNameModel()
                    {
                        Id = deck.Id,
                        Name = deck.Name
                    };
                    toAdd.Decks.Add(deckIdNameModel);
                }
            }
            return toReturn;
        }
    }
}
