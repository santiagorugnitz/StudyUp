namespace WebAPI.Models
{
    public class ResponseUserModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public bool IsStudent { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }
    }
}
