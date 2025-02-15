using BackTelega.Models;

namespace BackTelega.Services
{
    public interface IChatService
    {
        Task<Chat?> GetChatByIdAsync(int id);
        Task<IEnumerable<Chat>> GetAllChatsAsync();
        Task<bool> CreateChatAsync(string chatName, string chatType);
        Task<bool> DeleteChatAsync(int id);
    }
}
