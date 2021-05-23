﻿using Domain.Enumerations;

namespace WebAPI.Models
{
    public class UpdateDeckModel
    {
        public string Name { get; set; }
        public Difficulty Difficulty { get; set; }
        public bool IsHidden { get; set; }
        public string Subject { get; set; }
    }
}
