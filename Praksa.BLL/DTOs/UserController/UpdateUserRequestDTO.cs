using Praksa.DAL.Enums;

namespace Praksa.BLL.DTOs.UserController
{
    public class UpdateUserRequestDTO
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public Role UserRole { get; set; }
        public string? SecretId { get; set; }
    }
}
