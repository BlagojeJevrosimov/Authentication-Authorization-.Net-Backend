namespace Praksa.DAL.Entities
{
    public class PlatformCredentials : BaseEntity
    {
        public int UserId { get; set; }
        public string? Name { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }


        public PlatformCredentials()
        {
        }

        public PlatformCredentials(int id, DateTime createdAt, DateTime updatedAt, string? name, string? userName, string? password, int userId) :
            base(id, createdAt, updatedAt)
        {
            Name = name;
            UserName = userName;
            Password = password;
            UserId = userId;
        }
    }
}
