using BackTelega.Data;
using BackTelega.Models;
using Microsoft.EntityFrameworkCore;

namespace BackTelega.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly ClontelegramContext _context;

        public MessageRepository(ClontelegramContext context)
        {
            _context = context;
        }

        public async Task<Message?> GetByIdAsync(int id) =>
            await _context.Messages.FindAsync(id);

        public async Task<IEnumerable<Message>> GetByChatIdAsync(int chatId) =>
            await _context.Messages.Where(m => m.ChatId == chatId)
                                   .OrderByDescending(m => m.SentAt)
                                   .ToListAsync();

        public async Task AddAsync(Message message)
        {
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Message message)
        {
            _context.Messages.Update(message);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var message = await _context.Messages.FindAsync(id);
            if (message != null)
            {
                _context.Messages.Remove(message);
                await _context.SaveChangesAsync();
            }
        }
    }
}
