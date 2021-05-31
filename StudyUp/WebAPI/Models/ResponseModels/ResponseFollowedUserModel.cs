namespace WebAPI.Models
{
    public class ResponseFollowedUserModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public bool Following { get; set; }
        public bool IsStudent { get; set; }
        public string Token { get; set; }
        public string Username { get; set; }
    }
}
