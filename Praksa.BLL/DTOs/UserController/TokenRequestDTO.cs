using Praksa.DAL.Enums;

namespace Praksa.BLL.DTOs.UserController
{
    public class TokenRequestDTO
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public Role UserRole { get; set; }
    }
}
