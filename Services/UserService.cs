using BackTelega.Models;
using BackTelega.Repositories;
using System.Security.Cryptography;
using System.Text;

namespace BackTelega.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly TokenService _tokenService;

        public UserService(IUserRepository userRepository, TokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        public async Task<bool> RegisterUserAsync(string username, string email, string password)
        {
            if (await _userRepository.GetByEmailAsync(email) != null ||
                await _userRepository.GetByUsernameAsync(username) != null)
            {
                return false;
            }

            string hashedPassword = HashPassword(password);

            var user = new User
            {
                Username = username,
                Email = email,
                PasswordHash = hashedPassword,
                CreatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified)
            };

            await _userRepository.AddAsync(user);
            return true;
        }

        public async Task<string?> AuthenticateUserAsync(string username, string password)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            if (user == null || !VerifyPassword(password, user.PasswordHash))
            {
                return null;
            }

            return _tokenService.GenerateToken(user.Id, user.Username);
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            return Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(password)));
        }
        public async Task<User?> GetUserByIdAsync(int id) =>
                await _userRepository.GetByIdAsync(id);
        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return false;

            await _userRepository.DeleteAsync(id);
            return true;
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            return HashPassword(password) == hashedPassword;
        }
    }
}
