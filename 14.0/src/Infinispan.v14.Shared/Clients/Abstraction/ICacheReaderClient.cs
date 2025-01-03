using System.Net;
using Infinispan.v14.Shared.Models;

namespace Infinispan.v14.Shared.Clients.Abstraction;

internal interface ICacheReaderClient<T, TYpKey> 
    where T: CacheBaseModel where TYpKey: struct
{
    Task<T?> GetFromCacheAsync(TYpKey key, NetworkCredential credentials);
    Task<List<TYpKey>?> GetAllKeysFromCacheAsync(NetworkCredential credentials, int limit);
    Task<List<T>> GetByQueryFromCacheAsync(Func<T, bool> query, NetworkCredential credentials, int limit);
}