using Domain;

namespace WebAPI.Models
{
    public class GroupModel
    {
        public string Name { get; set; }
        public Group ToEntity() => new Group()
        {
            Name = this.Name
        };
    }
}
