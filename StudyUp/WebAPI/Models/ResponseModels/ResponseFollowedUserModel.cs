using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class ResponseFollowedUserModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public bool IsStudent { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }
        public bool Following { get; set; }
    }
}
