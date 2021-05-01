using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class GroupModel
    {
        public string Name { get; set; }
        public Group ToEntity() => new Group()
        {
            Name = this.Name
        };
    }
}
