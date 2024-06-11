using Crayon.API.Contracts;
using Crayon.API.Settings;
using Crayon.API.Util;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Crayon.API.Repositories
{
    public class RedisRepository : IRedisRepository
    {
        private readonly IDatabase redis;
        private readonly RedisSettings redisSettings;

        public RedisRepository(IConnectionMultiplexer muxer, IOptions<RedisSettings> redisSettings)
        {
            redis = muxer.GetDatabase();
            this.redisSettings = Guard.AgainstNull(redisSettings.Value, nameof(redisSettings));
        }


        public async Task<T> GetFromCache<T>(string cacheKey) where T : class
        {
            cacheKey = redisSettings.Version + cacheKey;
            var result = await redis.StringGetAsync(cacheKey);

            if (result.HasValue)
            {
                return JsonConvert.DeserializeObject<T>(result);

            }

            return null;

        }

        public async Task<List<T>> GetListFromCache<T>(string cacheKey) where T : class
        {
            cacheKey = redisSettings.Version + cacheKey;
            var result = await redis.StringGetAsync(cacheKey);

            if (result.HasValue)
            {
                return JsonConvert.DeserializeObject<List<T>>(result);

            }

            return null;

        }

        public async Task<bool> SaveToCache<T>(string cacheKey, T objectToSave, int cacheForHours, When when = When.Always) where T : class
        {
            if (cacheForHours == 0)
            {
                return false;
            }

            cacheKey = redisSettings.Version + cacheKey;

            var json = JsonConvert.SerializeObject(objectToSave);
            return await redis.StringSetAsync(cacheKey, json, expiry: TimeSpan.FromHours(cacheForHours), when);
        }


        public async Task<bool> SaveListToCache<T>(string cacheKey, List<T> listToSave, int cacheForHours, When when = When.Always) where T : class
        {
            if (cacheForHours == 0)
            {
                return false;
            }

            cacheKey = redisSettings.Version + cacheKey;

            var json = JsonConvert.SerializeObject(listToSave);
            return await redis.StringSetAsync(cacheKey, json, expiry: TimeSpan.FromHours(cacheForHours), when);
        }


        public async Task<bool> DeleteFromCache(string cacheKey)
            => await redis.KeyDeleteAsync(redisSettings.Version + cacheKey);
    }
}
