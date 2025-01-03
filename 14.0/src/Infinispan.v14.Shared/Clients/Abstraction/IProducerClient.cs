using Infinispan.v14.Shared.Models;

namespace Infinispan.v14.Shared.Clients.Abstraction;

public interface IProducerClient<in T, in TYpKey> 
    where T: CacheBaseModel where TYpKey: struct
{
    Task<bool> AddToCacheAsync(T model, TYpKey key);
    Task<bool> DeleteFromCacheAsync(TYpKey key);
}