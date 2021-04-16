using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Domain.Enumerations;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Models
{
    public class DeckModel
    {
        public string Name { get; set; }
        public Difficulty Difficulty { get; set; }
        public bool IsHidden { get; set; }
        public string Subject { get; set; }

        public Deck ToEntity() => new Deck()
        {
            Name = this.Name,
            Difficulty = this.Difficulty,
            IsHidden = this.IsHidden,
            Subject = this.Subject
        };
    }
}
