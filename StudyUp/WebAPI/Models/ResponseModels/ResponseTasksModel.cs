using System.Collections.Generic;

namespace WebAPI.Models.ResponseModels
{
    public class ResponseTaskDeckModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class ResponseTaskExamModel
    {
        public int Id { get; set; }
        public string GroupsName { get; set; }
        public string Name { get; set; }
    }
    public class ResponseTasksModel
    {
        public List<ResponseTaskExamModel> Exams { get; set; }
        public List<ResponseTaskDeckModel> Decks { get; set; }
    }
}
