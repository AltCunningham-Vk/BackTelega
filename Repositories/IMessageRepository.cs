using BackTelega.Models;

namespace BackTelega.Repositories
{
    public interface IMessageRepository
    {
        Task<Message?> GetByIdAsync(int id);
        Task<IEnumerable<Message>> GetByChatIdAsync(int chatId); // Проверяем наличие метода
        Task AddAsync(Message message);
        Task UpdateAsync(Message message);
        Task DeleteAsync(int id);
    }
}
