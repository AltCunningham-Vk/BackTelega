using BackTelega.Models;
using BackTelega.Repositories;

namespace BackTelega.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly RedisCacheService _cacheService;
        private readonly int _cacheMessageLimit = 50; // Храним последние 50 сообщений

        public MessageService(IMessageRepository messageRepository, RedisCacheService cacheService)
        {
            _messageRepository = messageRepository;
            _cacheService = cacheService;
        }

        public async Task<IEnumerable<Message>> GetMessagesByChatIdAsync(int chatId)
        {
            var cacheKey = $"chat_messages_{chatId}";
            var cachedMessages = await _cacheService.GetListAsync<Message>(cacheKey);

            if (cachedMessages.Count > 0)
                return cachedMessages;

            // Если в кеше нет сообщений — загружаем из БД
            var messages = await _messageRepository.GetByChatIdAsync(chatId);

            // Сохраняем в Redis
            foreach (var msg in messages)
            {
                await _cacheService.AddToListAsync(cacheKey, msg, _cacheMessageLimit);
            }

            return messages;
        }

        public async Task<bool> SendMessageAsync(int chatId, int senderId, string messageText, string? mediaType, string? mediaUrl)
        {
            if (string.IsNullOrWhiteSpace(messageText) && string.IsNullOrWhiteSpace(mediaUrl))
            {
                return false;
            }

            var message = new Message
            {
                ChatId = chatId,
                SenderId = senderId,
                MessageText = messageText,
                MediaType = mediaType,
                MediaUrl = mediaUrl,
                SentAt = DateTime.UtcNow
            };

            await _messageRepository.AddAsync(message);
            var cacheKey = $"chat_messages_{chatId}";
            await _cacheService.AddToListAsync(cacheKey, message, _cacheMessageLimit);
            return true;
        }

        public async Task<bool> UpdateMessageAsync(int messageId, string newText, string? newMediaType, string? newMediaUrl)
        {
            var message = await _messageRepository.GetByIdAsync(messageId);
            if (message == null) return false;

            message.MessageText = newText;
            message.MediaType = newMediaType;
            message.MediaUrl = newMediaUrl;

            await _messageRepository.UpdateAsync(message);
            return true;
        }

        public async Task<bool> DeleteMessageAsync(int messageId)
        {
            var message = await _messageRepository.GetByIdAsync(messageId);
            if (message == null) return false;

            await _messageRepository.DeleteAsync(messageId);

            // Очистка кеша, так как список мог измениться
            var cacheKey = $"chat_messages_{message.ChatId}";
            await _cacheService.RemoveCacheAsync(cacheKey);

            return true;
        }
    }
}
