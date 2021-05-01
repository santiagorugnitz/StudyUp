using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Models
{
    public class ResponseFlashcardModel
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }

    }
}
