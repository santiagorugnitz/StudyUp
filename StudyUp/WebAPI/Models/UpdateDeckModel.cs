using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Enumerations;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Models
{
    public class UpdateDeckModel
    {
        public string Name { get; set; }
        public Difficulty Difficulty { get; set; }
        public bool IsHidden { get; set; }
    }
}
