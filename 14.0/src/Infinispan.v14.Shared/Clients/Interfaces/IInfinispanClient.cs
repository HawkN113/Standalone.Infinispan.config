using System.Net;
using Infinispan.v14.Shared.Models;

namespace Infinispan.v14.Shared.Clients.Interfaces;

public interface IInfinispanClient<T, TYpKey> 
    where T: CacheBaseModel where TYpKey: struct
{
    Task<bool> AddToCacheAsync(T model, TYpKey key, NetworkCredential credentials);
    Task<T?> GetFromCacheAsync(TYpKey key, NetworkCredential credentials);
    Task<List<TYpKey>?> GetAllKeysFromCacheAsync(NetworkCredential credentials, int limit);
    Task<IEnumerable<T>> GetByQueryFromCacheAsync(Func<T, bool> query, NetworkCredential credentials, int limit);
    Task<bool> DeleteFromCacheAsync(TYpKey key, NetworkCredential credentials);
}