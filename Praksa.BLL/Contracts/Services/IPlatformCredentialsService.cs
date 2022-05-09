using Praksa.BLL.DTOs.PlatformCredentialsController;
using Praksa.DAL.Entities;

namespace Praksa.BLL.Contracts.Services
{
    public interface IPlatformCredentialsService : IBaseService<PlatformCredentials>
    {
        Task<IEnumerable<PlatformCredentials>> GetAllPlatformCredentialsByUserIdAsync(int userId, int requestUserId);
        Task<int> AddAsync(int userId, PlatformCredentials platformCredentials);
        Task<int> UpdateAsync(int userId, PlatformCredentials platformCredentials);
        Task DeleteAsync(int userId, int platformCredentialsId);
        Task<GetPlatformCredentialPasswordByIdResponseDTO> GetPlatformCredentialPasswordById(int platformCredentialId, int userId, string password);
    }
}
