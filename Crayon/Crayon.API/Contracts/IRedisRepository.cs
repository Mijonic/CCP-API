using StackExchange.Redis;

namespace Crayon.API.Contracts
{
    public interface IRedisRepository
    {
        Task<T> GetFromCache<T>(string cacheKey) where T : class;
        Task<List<T>> GetListFromCache<T>(string cacheKey) where T : class;
        Task<bool> SaveToCache<T>(string cacheKey, T objectToSave, int cacheForHours, When when = When.Always) where T : class;
        Task<bool> SaveListToCache<T>(string cacheKey, List<T> listToSave, int cacheForHours, When when = When.Always) where T : class;
        Task<bool> DeleteFromCache(string cacheKey);
    }
}
