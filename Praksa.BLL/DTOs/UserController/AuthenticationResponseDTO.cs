using Praksa.DAL.Entities;

namespace Praksa.BLL.DTOs.UserController
{
    public class AuthenticationResponseDTO
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Username { get; set; }
        public User User { get; }
        public string? Token { get; set; }


        public AuthenticationResponseDTO(User user, string token)
        {
            Id = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Username = user.UserName;
            User = user;
            Token = token;
        }
    }
}
