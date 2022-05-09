using Microsoft.Data.SqlClient;
using Praksa.DAL.Contracts.Repositories;
using Praksa.DAL.Entities;
using System.Data;

namespace Praksa.DAL.Repositories
{
    public class PlatformCredentialsRepository : BaseRepository<PlatformCredentials>, IPlatformCredentialsRepository
    {
        private readonly string _connectionString = "Server=(localdb)\\mssqllocaldb;Database=UserDbContext-dc847674-ccf4-4900-8b12-c1fee7a8e1e9;Trusted_Connection=True;MultipleActiveResultSets=true";
        
        
        public PlatformCredentialsRepository() : base(nameof(PlatformCredentials), "Server=(localdb)\\mssqllocaldb;Database=UserDbContext-dc847674-ccf4-4900-8b12-c1fee7a8e1e9;Trusted_Connection=True;MultipleActiveResultSets=true")
        {
        }


        protected override void InsertAddCommandParameters(SqlCommand command, PlatformCredentials entity)
        {
            base.InsertAddCommandParameters(command, entity);

            command.Parameters.AddWithValue("@UserId", entity.UserId);
            command.Parameters.AddWithValue("@Name", entity.Name);
            command.Parameters.AddWithValue("@UserName", entity.UserName);
            command.Parameters.AddWithValue("@Password", entity.Password);
        }
        protected override void InsertUpdateCommandParameters(SqlCommand command, PlatformCredentials entity)
        {
            base.InsertUpdateCommandParameters(command, entity);

            command.Parameters.AddWithValue("@UserId", entity.UserId);
            command.Parameters.AddWithValue("@Name", entity.Name);
            command.Parameters.AddWithValue("@UserName", entity.UserName);
            command.Parameters.AddWithValue("@Password", entity.Password);
        }
        protected override List<PlatformCredentials> Deserialize(SqlDataReader result)
        {
            List<PlatformCredentials> platformCredentials = new List<PlatformCredentials>();

            while (result.Read())
            {
                platformCredentials.Add(new PlatformCredentials()
                {
                    Id = result.GetInt32(0),
                    CreatedAt = result.GetDateTime(1),
                    UpdatedAt = result.GetDateTime(2),
                    UserId = result.GetInt32(3),
                    Name = result.GetString(4),
                    UserName = result.GetString(5),
                    Password = result.GetString(6)
                });
            }
            return platformCredentials;
        }

        public async Task<IEnumerable<PlatformCredentials>> GetAllByUserIdAsync(int userId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "spGetAllByUserId".Insert(8, nameof(PlatformCredentials));
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@UserId", userId);

                    var result = await command.ExecuteReaderAsync();

                    List<PlatformCredentials> entities = Deserialize(result);

                    return entities;
                }
            }
        }
    }
}
