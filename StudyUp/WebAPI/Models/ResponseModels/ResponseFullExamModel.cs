using Domain.Enumerations;
using System.Collections.Generic;

namespace WebAPI.Models.ResponseModels
{
    public class ResponseFullExamModel
    {
        public int Id { get; set; }
        public Difficulty Difficulty { get; set; }
        public IEnumerable<ResponseExamCardModel> Examcards { get; set; }
        public string GroupName { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }
    }
}
