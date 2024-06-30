using Microsoft.IdentityModel.Tokens;
using RandomApp1.Dtos;
using RandomApp1.Models;
using RandomApp1.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace RandomApp1.Services
{
    public interface ITokenService
    {
        public TokenResDto GetToken(IdentityUser user);

        public string GenerateRefreshToken();

        public Task<int> SaveRefreshToken(string token, int userId);

        public Task<RefreshToken> GetRefreshToken(string token);

        public Task<int> DeleteRefreshToken(string token);
    }

    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey key;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly string issuer;
        private readonly string audience;

        public TokenService(IConfiguration config, IRefreshTokenRepository refreshTokenRepository)
        {
            key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
            issuer = config["TokenIssuer"];
            audience = config["TokenAudience"];
            _refreshTokenRepository = refreshTokenRepository;
        }

        public TokenResDto GetToken(IdentityUser user)
        {
            var claims = new List<Claim>
            {
               new Claim(JwtRegisteredClaimNames.NameId, user.Username),
               new Claim(JwtRegisteredClaimNames.Email, user.Email),
            };
            var expiration = DateTime.UtcNow.AddSeconds(15);
            var signInCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var tokenOptions = new JwtSecurityToken(issuer: issuer, audience: audience, claims: claims, expires: DateTime.Now.AddSeconds(30),
                signingCredentials: signInCredentials);

            var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            var tokenRes = new TokenResDto
            {
                Expiration = expiration,
                Token = token
            };
            return tokenRes;
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
            }
            return Convert.ToBase64String(randomNumber);
        }

        public async Task<int> SaveRefreshToken(string token, int userId)
        {
            var refreshToken = new RefreshToken()
            {
                UserId = userId,
                Token = token,
                ExpiryDate = DateTime.Now.AddHours(1)
            };

            return await _refreshTokenRepository.SaveRefreshTokenAsync(refreshToken);
        }

        public async Task<RefreshToken> GetRefreshToken(string token)
        {
            return await _refreshTokenRepository.GetRefreshToken(token);
        }

        public async Task<int> DeleteRefreshToken(string token)
        {
            return await _refreshTokenRepository.DeleteRefreshToken(token);
        }
    }
}