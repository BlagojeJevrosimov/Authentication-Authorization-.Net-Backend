using Praksa.BLL.DTOs.UserController;
using Praksa.DAL.Entities;
using Praksa.DAL.Enums;

namespace Praksa.BLL.Contracts.Services
{
    public interface IUserService : IBaseService<User>
    {
        Task<User> GetUserByIdAsync(int userId, Role userRole, int id);
        Task DeleteUserAsync(int id);
        Task UpdateUserAsync(int userId, Role userRole, UpdateUserRequestDTO request);
        Task<RegisterUserResponseDTO> RegisterUserAsync(RegisterUserRequestDTO request);
        Task<LoginResponseDTO> Login(LoginRequestDTO request);
        string RefreshToken(int userId, string userName, Role userRole);
        Task<bool> PasswordCheck(int userId, string password);
    }
}
