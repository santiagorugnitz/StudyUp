using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public bool IsATeacher { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }

        public User ToEntity() => new User()
        {
            Email = this.Email,
            IsATeacher = this.IsATeacher,
            Name = this.Name,
            Password = this.Password
        };

    }
}
