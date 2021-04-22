﻿using System;
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
                loguedUser = logic.LoginByUsername(loginModel.Username, loginModel.Password);
            } 
            else
            {
                loguedUser = logic.Login(loginModel.Email, loginModel.Password);
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

        [HttpPost("/api/users/follow")]
        public IActionResult FollowUser([FromHeader] string token, [FromQuery] string username)
        {
            return Ok(logic.FollowUser(token, username));
        }

        [HttpDelete("/api/users/unfollow")]
        public IActionResult UnfollowUser([FromHeader] string token, [FromQuery] string username)
        {
            return Ok(logic.UnfollowUser(token, username));
        }

        [HttpGet]
        public IActionResult GetUsers([FromHeader] string token, [FromQuery] string username = "")
        {
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

    }
}
