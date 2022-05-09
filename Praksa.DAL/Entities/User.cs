using Praksa.DAL.Enums;

namespace Praksa.DAL.Entities
{
    public class User : BaseEntity
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public Role UserRole { get; set; }
        public string? SecretId { get; set; }


        public User() : base()
        {
        }

        public User(int id, DateTime createdAt, DateTime updatedAt, string? firstName, string? lastName, string? userName, string? email, string? password, Role userRole, string secretId) :
            base(id, createdAt, updatedAt)
        {
            FirstName = firstName;
            LastName = lastName;
            UserName = userName;
            Email = email;
            Password = password;
            UserRole = userRole;
            SecretId = secretId;

        }
    }
}
