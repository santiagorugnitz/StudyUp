using System.Collections.Generic;

namespace WebAPI.Models
{
    public class ResponseTeachersGroupModel
    {
        public int Id { get; set; }
        public List<ResponseDeckIdNameModel> Decks { get; set; }
        public string Name { get; set; }
    }
}
