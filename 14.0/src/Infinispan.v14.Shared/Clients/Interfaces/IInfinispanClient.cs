using System.Net;
using Infinispan.v14.Shared.Models;

namespace Infinispan.v14.Shared.Clients.Interfaces;

public interface IInfinispanClient<in TIn, TYpKey, TOut> 
    where TIn: CacheBaseModel where TYpKey: struct where TOut: CacheBaseModel
{
    Task<bool> AddToCacheAsync(TIn model, TYpKey key, string cacheName, NetworkCredential credentials);
    Task<TOut?> GetFromCacheAsync(TYpKey key, string cacheName, NetworkCredential credentials);
    Task<List<TYpKey>?> GetAllKeysFromCacheAsync(string cacheName, NetworkCredential credentials, int limit);
    Task<bool> DeleteFromCacheAsync(TYpKey key, string cacheName, NetworkCredential credentials);
}