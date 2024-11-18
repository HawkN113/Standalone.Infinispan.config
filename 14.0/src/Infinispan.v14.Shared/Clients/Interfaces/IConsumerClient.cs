using Infinispan.v14.Shared.Models;

namespace Infinispan.v14.Shared.Clients.Interfaces;

public interface IConsumerClient<TYpKey, T> 
    where TYpKey: struct where T: CacheBaseModel
{
    Task<T?> GetFromCacheAsync(TYpKey key);
    Task<List<TYpKey>?> GetAllKeysFromCacheAsync(int limit);
    Task<IEnumerable<T>> GetByQueryFromCacheAsync(Func<T, bool> query, int limit);
}