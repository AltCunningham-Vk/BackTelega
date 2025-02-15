using BackTelega.Models;

namespace BackTelega.Repositories
{
    public interface IChatRepository
    {
        Task<Chat?> GetByIdAsync(int id);
        Task<IEnumerable<Chat>> GetAllAsync();
        Task AddAsync(Chat chat);
        Task UpdateAsync(Chat chat);
        Task DeleteAsync(int id);
    }
}
