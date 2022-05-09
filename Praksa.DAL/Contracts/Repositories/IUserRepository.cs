using Praksa.DAL.Entities;

namespace Praksa.DAL.Contracts.Repositories
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User> GetByUsernameAsync(string username);
    }
}
