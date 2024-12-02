using System.Net;
using Infinispan.v14.Shared.Models;

namespace Infinispan.v14.Shared.Clients.Abstraction;

internal interface ICacheWriterClient<T, TYpKey> 
    where T: CacheBaseModel where TYpKey: struct
{
    Task<bool> AddToCacheAsync(T model, TYpKey key, NetworkCredential credentials);
    Task<bool> DeleteFromCacheAsync(TYpKey key, NetworkCredential credentials);
}