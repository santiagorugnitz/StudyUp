using Domain;

namespace WebAPI.Models.ResponseModels
{
    public class ResponseExamCardModel
    {
        public int Id { get; set; }
        public bool Answer { get; set; }
        public string Question { get; set; }
    }
}
