using Dapper;
using RandomApp1.Data;
using RandomApp1.Models;

namespace RandomApp1.Repositories
{
    public interface IRefreshTokenRepository
    {
        public Task<int> SaveRefreshTokenAsync(RefreshToken refreshToken);

        public Task<RefreshToken> GetRefreshToken(string token);

        public Task<int> DeleteRefreshToken(string token);
    }

    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly DataContext _db;

        public RefreshTokenRepository(DataContext db)
        {
            _db = db;
        }

        public async Task<int> DeleteRefreshToken(string token)
        {
            string query = "DELETE FROM RefreshTokens WHERE Token = @Token";

            var parameters = new
            {
                Token = token,
            };

            using var connection = _db.createConnection();
            return await connection.ExecuteAsync(query, parameters);
        }

        public async Task<RefreshToken> GetRefreshToken(string token)
        {
            string query = "SELECT * FROM RefreshTokens WHERE Token = @Token AND ExpiryDate > GETUTCDATE()";

            var parameters = new
            {
                Token = token,
            };

            using var connection = _db.createConnection();
            return await connection.QueryFirstOrDefaultAsync<RefreshToken>(query, parameters);
        }

        public async Task<int> SaveRefreshTokenAsync(RefreshToken refreshToken)
        {
            string query = "INSERT INTO RefreshTokens(Token, UserId, ExpiryDate) values (@Token, @UserId, @ExpiryDate); SELECT CAST(SCOPE_IDENTITY() as int)";

            var parameters = new
            {
                Token = refreshToken.Token,
                UserId = refreshToken.UserId,
                ExpiryDate = refreshToken.ExpiryDate
            };

            using var connection = _db.createConnection();
            return await connection.QuerySingleAsync<int>(query, parameters);
        }
    }
}