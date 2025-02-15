using BackTelega.Models;

namespace BackTelega.Services
{
    public interface IUserService
    {
        Task<bool> RegisterUserAsync(string username, string email, string password);
        Task<string?> AuthenticateUserAsync(string username, string password);
        Task<User?> GetUserByIdAsync(int id); // Добавили этот метод
        Task<bool> DeleteUserAsync(int id); // Добавили этот метод

    }
}
