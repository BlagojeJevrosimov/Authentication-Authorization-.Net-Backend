using Praksa.BLL.Contracts.Services;
using Praksa.DAL.Contracts.Repositories;
using Praksa.DAL.Entities;

namespace Praksa.BLL.Services
{
    public class BaseService<TEntity> : IBaseService<TEntity> where TEntity : BaseEntity
    {
        protected readonly IBaseRepository<TEntity> _repository;


        public BaseService(IBaseRepository<TEntity> repository)
        {
            _repository = repository;

        }


        public async Task<int> AddAsync(TEntity entity)
        {
            return await _repository.AddAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteByIdAsync(id);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<int> UpdateAsync(TEntity entity)
        {
            return await _repository.UpdateAsync(entity);
        }
    }
}
