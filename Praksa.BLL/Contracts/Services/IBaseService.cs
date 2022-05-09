using Praksa.DAL.Entities;

namespace Praksa.BLL.Contracts.Services
{
    public interface IBaseService<TEntity> where TEntity : BaseEntity
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity> GetByIdAsync(int id);
        Task DeleteAsync(int id);
        Task<int> UpdateAsync(TEntity entity);
        Task<int> AddAsync(TEntity entity);
    }
}
