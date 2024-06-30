using Dapper;
using RandomApp1.Data;
using RandomApp1.Models;

namespace RandomApp1.Repositories
{
    public interface IIdentityRepository
    {
        Task<int> Register(IdentityUser user);

        Task<IdentityUser> GetUserById(int id);

        Task<IdentityUser> GetUserByEmail(string email);
    }

    public class IdentityRepository : IIdentityRepository
    {
        private readonly DataContext db;

        public IdentityRepository(DataContext dataContext)
        {
            db = dataContext;
        }

        public async Task<int> Register(IdentityUser user)
        {
            string query = "INSERT INTO Users (Email, Username, PasswordHash, PasswordSalt, CreatedTime) VALUES (@Email, @Username, @PasswordHash, @PasswordSalt, @CreatedTime); SELECT CAST(SCOPE_IDENTITY() as int)";

            var parameters = new
            {
                Email = user.Email,
                Username = user.Username,
                PasswordHash = user.PasswordHash,
                PasswordSalt = user.PasswordSalt,
                CreatedTime = user.CreatedTime
            };

            using var connection = db.createConnection();
            return await connection.QuerySingleAsync<int>(query, parameters);
        }

        public async Task<IdentityUser> GetUserById(int id)
        {
            string query = "SELECT * FROM Users WHERE Id = @Id";
            var parameters = new
            {
                Id = id
            };

            using var connection = db.createConnection();
            return await connection.QueryFirstOrDefaultAsync<IdentityUser>(query, parameters);
        }

        public async Task<IdentityUser> GetUserByEmail(string email)
        {
            string query = "SELECT * FROM Users WHERE Email = @Email";
            var parameters = new
            {
                Email = email
            };

            using var connection = db.createConnection();
            return await connection.QueryFirstOrDefaultAsync<IdentityUser>(query, parameters);
        }
    }
}