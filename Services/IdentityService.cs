using RandomApp1.Dtos;
using RandomApp1.Models;
using RandomApp1.Repositories;
using System.Security.Cryptography;
using System.Text;

namespace RandomApp1.Services
{
    public interface IIdentityService
    {
        Task<int> Register(RegisterUserDto registerUserDto);

        bool Login(LoginDto loginDto, IdentityUser user);

        Task<IdentityUser> GetUserById(int id);

        Task<IdentityUser> GetUserByEmail(string email);
    }

    public class IdentityService : IIdentityService
    {
        private readonly IIdentityRepository _identityRepository;

        public IdentityService(IIdentityRepository identityRepository)
        {
            _identityRepository = identityRepository;
        }

        public async Task<int> Register(RegisterUserDto registerUserDto)
        {
            using var hmac = new HMACSHA256();
            var user = new IdentityUser
            {
                Email = registerUserDto.Email,
                Username = registerUserDto.Username,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerUserDto.Password)),
                PasswordSalt = hmac.Key,
                CreatedTime = DateTime.UtcNow
            };

            return await _identityRepository.Register(user);
        }

        public bool Login(LoginDto loginDto, IdentityUser user)
        {
            using var hmac = new HMACSHA256(user.PasswordSalt);
            var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
            if (computeHash.Length != user.PasswordHash.Length)
            {
                return false;
            }
            // Don't convert this to forEach
            for (int i = 0; i < computeHash.Length; i++)
            {
                if (computeHash[i] != user.PasswordHash[i]) return false;
            }

            return true;
        }

        public async Task<IdentityUser> GetUserById(int id)
        {
            return await _identityRepository.GetUserById(id);
        }

        public async Task<IdentityUser> GetUserByEmail(string email)
        {
            return await _identityRepository.GetUserByEmail(email);
        }
    }
}