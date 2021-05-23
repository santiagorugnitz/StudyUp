using Domain;

namespace WebAPI.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public bool IsStudent { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public string FirebaseToken { get; set; }

        public User ToEntity() => new User()
        {
            Email = this.Email,
            IsStudent = this.IsStudent,
            Username = this.Username,
            Password = this.Password,
            Token = this.Token,
            FirebaseToken = this.FirebaseToken
        };

    }
}
