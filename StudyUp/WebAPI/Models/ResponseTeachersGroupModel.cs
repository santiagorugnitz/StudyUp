using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class ResponseTeachersGroupModel
    {
        public string Name { get; set; }
        public List<ResponseDeckIdNameModel> Decks { get; set; }
    }
}
