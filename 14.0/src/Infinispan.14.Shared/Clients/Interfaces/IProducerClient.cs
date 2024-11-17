using Infinispan._14.Shared.Models;

namespace Infinispan._14.Shared.Clients.Interfaces;

public interface IProducerClient<in TIn, in TYpKey> 
    where TIn: CacheBaseModel where TYpKey: struct
{
    Task<bool> AddToCacheAsync(TIn model, TYpKey key);
    Task<bool> DeleteFromCacheAsync(TYpKey key);
}