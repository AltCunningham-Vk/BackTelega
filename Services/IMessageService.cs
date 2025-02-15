using BackTelega.Models;

namespace BackTelega.Services
{
    public interface IMessageService
    {
        Task<IEnumerable<Message>> GetMessagesByChatIdAsync(int chatId); // Должно быть так!
        Task<bool> SendMessageAsync(int chatId, int senderId, string messageText, string? mediaType, string? mediaUrl);
        Task<bool> UpdateMessageAsync(int messageId, string newText, string? newMediaType, string? newMediaUrl);
        Task<bool> DeleteMessageAsync(int id);
    }
}
