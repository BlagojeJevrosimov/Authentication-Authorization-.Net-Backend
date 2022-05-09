using Praksa.DAL.Entities;

namespace Praksa.DAL.Contracts.Repositories
{
    public interface IPlatformCredentialsRepository : IBaseRepository<PlatformCredentials>
    {
        Task<IEnumerable<PlatformCredentials>> GetAllByUserIdAsync(int userId);
    }
}
