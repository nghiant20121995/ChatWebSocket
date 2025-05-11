using ChatWebSocket.Domain.Interfaces.Cache;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace ChatWebSocket.Infrastructure.Cache
{
    public class CacheClient : ICacheClient
    {
        private readonly IDatabase _db;
        public CacheClient(IConnectionMultiplexer connectionMultiplexer)
        {
            _db = connectionMultiplexer.GetDatabase();
        }

        public async Task SetStringAsync(string key, string value, TimeSpan? expiry = null)
        {
            await _db.StringSetAsync(key, value, expiry);
        }

        public async Task<string> GetStringAsync(string key)
        {
            return await _db.StringGetAsync(key);
        }

        public string GetString(string key)
        {
            return _db.StringGet(key);
        }
    }
}
