using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace BackTelega.Services
{
    public class RedisCacheService
    {
        private readonly IDatabase _cache;
        private readonly TimeSpan _cacheExpiry = TimeSpan.FromMinutes(10); // Кеш сообщений на 10 минут

        public RedisCacheService(IConnectionMultiplexer redis)
        {
            _cache = redis.GetDatabase();
        }

        public async Task SetCacheAsync<T>(string key, T data, TimeSpan expiry)
        {
            var jsonData = JsonSerializer.Serialize(data);
            await _cache.StringSetAsync(key, jsonData, expiry);
        }

        public async Task<T?> GetCacheAsync<T>(string key)
        {
            var data = await _cache.StringGetAsync(key);
            return data.HasValue ? JsonSerializer.Deserialize<T>(data!) : default;
        }

        public async Task RemoveCacheAsync(string key)
        {
            await _cache.KeyDeleteAsync(key);
        }

        // Метод для работы со списками (например, кеш сообщений)
        public async Task AddToListAsync<T>(string key, T item, int maxSize)
        {
            var jsonData = JsonSerializer.Serialize(item);
            await _cache.ListRightPushAsync(key, jsonData);

            // Ограничиваем список, удаляя старые записи
            await _cache.ListTrimAsync(key, -maxSize, -1);
        }

        public async Task<List<T>> GetListAsync<T>(string key)
        {
            var messages = await _cache.ListRangeAsync(key);
            var result = new List<T>();

            foreach (var message in messages)
            {
                result.Add(JsonSerializer.Deserialize<T>(message!)!);
            }

            return result;
        }
    }
}
