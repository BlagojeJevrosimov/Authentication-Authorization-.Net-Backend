using Microsoft.Data.SqlClient;
using Praksa.DAL.Contracts.Repositories;
using Praksa.DAL.Entities;
using Praksa.DAL.Enums;
using System.Data;

namespace Praksa.DAL.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly string _connectionString = "Server=(localdb)\\mssqllocaldb;Database=UserDbContext-dc847674-ccf4-4900-8b12-c1fee7a8e1e9;Trusted_Connection=True;MultipleActiveResultSets=true";
        
        
        public UserRepository() : base(nameof(User), "Server=(localdb)\\mssqllocaldb;Database=UserDbContext-dc847674-ccf4-4900-8b12-c1fee7a8e1e9;Trusted_Connection=True;MultipleActiveResultSets=true")
        {

        }


        public async Task<User> GetByUsernameAsync(string username)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "spGetUserByUsername";
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@UserName", username);

                    var result = await command.ExecuteReaderAsync();

                    if (result.Read())
                    {
                        Enum.TryParse(typeof(Role), result.GetInt32(8).ToString(), out var role);

                        return new User(result.GetInt32(0), result.GetDateTime(1), result.GetDateTime(2), result.GetString(3), 
                            result.GetString(4), result.GetString(5), result.GetString(6), result.GetString(7), (Role)role!, result.GetString(9));
                    }
                }
            }
            return null!;
        }
        protected override void InsertAddCommandParameters(SqlCommand command, User entity)
        {
            base.InsertAddCommandParameters(command, entity);

            command.Parameters.AddWithValue("@UserName", entity.UserName);
            command.Parameters.AddWithValue("@FirstName", entity.FirstName);
            command.Parameters.AddWithValue("@LastName", entity.LastName);
            command.Parameters.AddWithValue("@Password", entity.Password);
            command.Parameters.AddWithValue("@Email", entity.Email);
            command.Parameters.AddWithValue("@UserRole", (Int32)entity.UserRole);
            command.Parameters.AddWithValue("@SecretId", entity.SecretId);
        }
        protected override void InsertUpdateCommandParameters(SqlCommand command, User entity)
        {
            base.InsertUpdateCommandParameters(command, entity);

            command.Parameters.AddWithValue("@UserName", entity.UserName);
            command.Parameters.AddWithValue("@FirstName", entity.FirstName);
            command.Parameters.AddWithValue("@LastName", entity.LastName);
            command.Parameters.AddWithValue("@Password", entity.Password);
            command.Parameters.AddWithValue("@Email", entity.Email);
            command.Parameters.AddWithValue("@UserRole", (Int32)entity.UserRole);
            command.Parameters.AddWithValue("@SecretId", entity.SecretId);
        }
        protected override List<User> Deserialize(SqlDataReader result)
        {
            List<User> users = new List<User>();

            while (result.Read())
            {
                users.Add(new User()
                {
                    Id = result.GetInt32(0),
                    CreatedAt = result.GetDateTime(1),
                    UpdatedAt = result.GetDateTime(2),
                    FirstName = result.GetString(3),
                    LastName = result.GetString(4),
                    UserName = result.GetString(5),
                    Email = result.GetString(6),
                    Password = result.GetString(7),
                    UserRole = (Role)Enum.Parse(typeof(Role), result.GetInt32(8).ToString()),
                    SecretId = result.GetString(9)
                });
            }
            return users;
        }
    }
}
