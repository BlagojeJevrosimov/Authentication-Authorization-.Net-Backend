namespace Praksa.BLL.DTOs.UserController
{
    public class RefreshTokenResponseDTO
    {
        public string? Token { get; set; }


        public RefreshTokenResponseDTO(string token)
        {
            Token = token;
        }
    }
}
