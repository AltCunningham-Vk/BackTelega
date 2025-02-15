using BackTelega.Models;
using BackTelega.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BackTelega.Services
{
    public class ChatService : IChatService
    {
        private readonly IChatRepository _chatRepository;
        private readonly RedisCacheService _cacheService;
        private readonly TimeSpan _cacheExpiry = TimeSpan.FromMinutes(5);

        public ChatService(IChatRepository chatRepository, RedisCacheService cacheService)
        {
            _chatRepository = chatRepository;
            _cacheService = cacheService;
        }

        public async Task<Chat?> GetChatByIdAsync(int id) =>
            await _chatRepository.GetByIdAsync(id);

        public async Task<IEnumerable<Chat>> GetAllChatsAsync() =>
            await _chatRepository.GetAllAsync();

        public async Task<bool> CreateChatAsync(string chatName, string chatType)
        {
            if (chatType != "private" && chatType != "group")
            {
                return false; // Неверный тип чата
            }

            var chat = new Chat
            {
                ChatName = chatName,
                ChatType = chatType
                //CreatedAt = DateTime.UtcNow
            };

            await _chatRepository.AddAsync(chat);
            return true;
        }

        public async Task<bool> DeleteChatAsync(int id)
        {
            await _chatRepository.DeleteAsync(id);
            return true;
        }
    }
}
