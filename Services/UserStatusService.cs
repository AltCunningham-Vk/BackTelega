using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace BackTelega.Services
{
    public class UserStatusService
    {
        private readonly IDatabase _cache;
        private readonly TimeSpan _statusExpiry = TimeSpan.FromMinutes(10); // Онлайн-статус хранится 10 минут

        public UserStatusService(IConnectionMultiplexer redis)
        {
            _cache = redis.GetDatabase();
        }

        public async Task SetUserOnlineAsync(int userId)
        {
            await _cache.StringSetAsync($"user_online_{userId}", "online", _statusExpiry);
        }

        public async Task<bool> IsUserOnlineAsync(int userId)
        {
            return await _cache.KeyExistsAsync($"user_online_{userId}");
        }
    }
}
