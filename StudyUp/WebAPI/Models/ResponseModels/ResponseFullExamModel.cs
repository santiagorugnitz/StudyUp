using Domain.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models.ResponseModels
{
    public class ResponseFullExamModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Difficulty Difficulty { get; set; }
        public string Subject { get; set; }
        public string GroupName { get; set; }
        public IEnumerable<ResponseExamCardModel> Examcards { get; set; }
    }
}
