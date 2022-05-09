namespace Praksa.BLL.DTOs.UserController
{
    public class LoginResponseDTO
    {
        public string? Token { get; set; }

        public LoginResponseDTO(string? token)
        {
            Token = token;
        }
    }
}
