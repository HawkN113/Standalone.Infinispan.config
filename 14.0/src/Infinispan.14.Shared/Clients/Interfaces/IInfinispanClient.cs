using System.Net;
using Infinispan._14.Shared.Model;

namespace Infinispan._14.Shared.Clients.Interfaces;

public interface IInfinispanClient<in TIn, TYpKey, TOut> 
    where TIn: CacheBaseModel where TYpKey: struct where TOut: CacheBaseModel
{
    Task<bool> AddToCacheAsync(TIn model, TYpKey key, string cacheName, NetworkCredential credentials);
    Task<TOut?> GetFromCacheAsync(TYpKey key, string cacheName, NetworkCredential credentials);
    Task<List<TYpKey>?> GetAllFromCacheAsync(string cacheName, NetworkCredential credentials);
    Task<bool> DeleteFromCacheAsync(TYpKey key, string cacheName, NetworkCredential credentials);
}