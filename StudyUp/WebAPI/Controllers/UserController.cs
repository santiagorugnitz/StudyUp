using System.Collections.Generic;
using System.Linq;
using BusinessLogicInterface;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;
using Domain;
using WebAPI.Filters;
using WebAPI.Models.ResponseModels;

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

        [HttpPost("/login")]
        public IActionResult Login([FromBody] LoginModel loginModel)
        {
            User loguedUser = new User();

            if (loginModel.Username != null && loginModel.Username.Trim().Length > 0)
            {
                loguedUser = logic.LoginByUsername(loginModel.Username, loginModel.Password, loginModel.FirebaseToken);
            }
            else
            {
                loguedUser = logic.Login(loginModel.Email, loginModel.Password, loginModel.FirebaseToken);
            }

            ResponseUserModel responseUserModel = new ResponseUserModel()
            {
                Id = loguedUser.Id,
                Email = loguedUser.Email,
                IsStudent = loguedUser.IsStudent,
                Username = loguedUser.Username,
                Token = loguedUser.Token
            };
            return Ok(responseUserModel);
        }

        [HttpDelete("/logout")]
        public IActionResult Logout([FromHeader] string token)
        {
            logic.Logout(token);
            return Ok();
        }

        [HttpPost("/api/users/follow")]
        public IActionResult FollowUser([FromHeader] string token, [FromQuery] string username)
        {
            logic.FollowUser(token, username);
            return Ok();
        }

        [HttpDelete("/api/users/unfollow")]
        public IActionResult UnfollowUser([FromHeader] string token, [FromQuery] string username)
        {
            logic.UnfollowUser(token, username);
            return Ok();
        }

        [HttpGet]
        public IActionResult GetUsers([FromHeader] string token, [FromQuery] string username = "")
        {
            if (username == null)
            {
                username = "";
            }

            var userList = logic.GetUsers(token, username);
            List<ResponseFollowedUserModel> responseList = new List<ResponseFollowedUserModel>();

            foreach (var tuple in userList)
            {
                ResponseFollowedUserModel responseUserModel = new ResponseFollowedUserModel()
                {
                    Id = tuple.Item1.Id,
                    Email = tuple.Item1.Email,
                    IsStudent = tuple.Item1.IsStudent,
                    Username = tuple.Item1.Username,
                    Token = tuple.Item1.Token,
                    Following = tuple.Item2
                };

                responseList.Add(responseUserModel);
            }

            return Ok(responseList);
        }

        [HttpGet("/api/decks/following")]
        public IActionResult GetFollowingDecks([FromHeader] string token)
        {
            var deckList = logic.GetDecksFromFollowing(token);
            List<ResponseDeckModel> responseList = new List<ResponseDeckModel>();

            foreach (var deck in deckList)
            {
                if (!deck.IsHidden && deck.Flashcards.Count > 0)
                {
                    ResponseDeckModel responseDeck = new ResponseDeckModel()
                    {
                        Id = deck.Id,
                        Author = deck.Author.Username,
                        Name = deck.Name,
                        Subject = deck.Subject,
                        Difficulty = deck.Difficulty,
                        IsHidden = deck.IsHidden
                    };
                    responseList.Add(responseDeck);
                }
            }
            return Ok(responseList);
        }

        [HttpGet("ranking")]
        public IActionResult Ranking([FromHeader] string token)
        {
            List<User> followedUsers = logic.GetUsersForRanking(token);
            List<ResponseRankingModel> toReturn = new List<ResponseRankingModel>();
            if (!(followedUsers is null))
            {
                foreach (User user in followedUsers)
                {
                    ResponseRankingModel toAdd = new ResponseRankingModel()
                    {
                        Username = user.Username,
                        Score = logic.GetScore(user.Username)
                    };
                    toReturn.Add(toAdd);
                }
            }
            return Ok(toReturn.OrderBy(a => a.Score));
        }
    }
}
