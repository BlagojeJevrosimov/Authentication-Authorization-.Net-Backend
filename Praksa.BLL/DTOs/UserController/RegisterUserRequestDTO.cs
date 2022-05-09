using Praksa.DAL.Enums;

namespace Praksa.BLL.DTOs.UserController
{
    public class RegisterUserRequestDTO
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public Role UserRole { get; set; }
    }
}
