using BackTelega.Data;
using BackTelega.Models;
using Microsoft.EntityFrameworkCore;

namespace BackTelega.Repositories
{
    public class ChatRepository : IChatRepository
    {
        private readonly ClontelegramContext _context;

        public ChatRepository(ClontelegramContext context)
        {
            _context = context;
        }

        public async Task<Chat?> GetByIdAsync(int id) =>
            await _context.Chats.FindAsync(id);

        public async Task<IEnumerable<Chat>> GetAllAsync() =>
            await _context.Chats.ToListAsync();

        public async Task AddAsync(Chat chat)
        {
            _context.Chats.Add(chat);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Chat chat)
        {
            _context.Chats.Update(chat);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var chat = await _context.Chats.FindAsync(id);
            if (chat != null)
            {
                _context.Chats.Remove(chat);
                await _context.SaveChangesAsync();
            }
        }
    }
}
