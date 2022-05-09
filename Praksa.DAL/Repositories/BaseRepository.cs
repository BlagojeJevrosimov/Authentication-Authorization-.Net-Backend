using Microsoft.Data.SqlClient;
using Praksa.DAL.Contracts.Repositories;
using Praksa.DAL.Entities;
using System.Data;

namespace Praksa.DAL.Repositories
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity, new()
    {
        private readonly string _connectionString;
        private readonly string _entityName;


        public BaseRepository(string entityName, string connectionString)
        {
            _entityName = entityName;
            _connectionString = connectionString;
        }


        public async Task<int> AddAsync(TEntity entity)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "spAdd".Insert(5, _entityName);
                    command.CommandType = CommandType.StoredProcedure;

                    InsertAddCommandParameters(command, entity);

                    await command.ExecuteNonQueryAsync();

                    var Id = (Int32)command.Parameters["@Id"].Value;

                    return Id;
                }
            }
        }

        public async Task DeleteByIdAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "spDeleteById".Insert(8, _entityName);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Id", id);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "spGetAll".Insert(8, _entityName);
                    command.CommandType = CommandType.StoredProcedure;

                    var result = await command.ExecuteReaderAsync();

                    List<TEntity> entities = Deserialize(result);

                    return entities;
                }
            }
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "spGetById".Insert(5, _entityName);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Id", id);

                    var result = await command.ExecuteReaderAsync();

                    List<TEntity> entities = Deserialize(result);

                    if (entities.Count == 0)
                    {
                        return null!;
                    }

                    return entities.First();
                }
            }

        }

        public async Task<int> UpdateAsync(TEntity entity)
        {
            if (EntityExists(entity.Id))
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "spUpdate".Insert(8, _entityName);
                        command.CommandType = CommandType.StoredProcedure;

                        InsertUpdateCommandParameters(command, entity);

                        await command.ExecuteNonQueryAsync();

                        return (Int32)command.Parameters["@Id"].Value;
                    }
                }
            }
            return -1;
        }

        protected bool EntityExists(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "spGetById".Insert(5, _entityName);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Id", id);

                    var result = command.ExecuteReader();

                    if (result.Read())
                    {
                        return true;
                    }

                    return false;
                }
            }

        }

        protected virtual List<TEntity> Deserialize(SqlDataReader result)
        {
            List<TEntity> list = new List<TEntity>();

            while (result.Read())
            {
                list.Add(new TEntity()
                {
                    Id = result.GetInt32(0),
                    CreatedAt = result.GetDateTime(1),
                    UpdatedAt = result.GetDateTime(2)
                });
            }

            return list;
        }

        protected virtual void InsertAddCommandParameters(SqlCommand command, TEntity entity)
        {
            command.Parameters.AddWithValue("@CreatedAt", DateTime.UtcNow);
            command.Parameters.AddWithValue("@UpdatedAt", DateTime.UtcNow);

            command.Parameters.Add(new SqlParameter()
            {
                Direction = ParameterDirection.Output,
                DbType = DbType.Int32,
                ParameterName = "@Id"
            });
        }

        protected virtual void InsertUpdateCommandParameters(SqlCommand command, TEntity entity)
        {
            command.Parameters.AddWithValue("@UpdatedAt", DateTime.UtcNow);
            command.Parameters.AddWithValue("@Id", entity.Id);
        }
    }
}
