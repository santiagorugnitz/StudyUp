using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Models
{
    public class FlashcardModel
    {
        public int Id { get; set; }
        public Deck Deck { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }

        public Flashcard ToEntity() => new Flashcard()
        {
           Id = this.Id,
           Deck = this.Deck,
           Question = this.Question,
           Answer = this.Answer
        };
    }
}
