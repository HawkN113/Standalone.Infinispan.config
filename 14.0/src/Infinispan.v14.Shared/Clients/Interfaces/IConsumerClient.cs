using Infinispan.v14.Shared.Models;

namespace Infinispan.v14.Shared.Clients.Interfaces;

public interface IConsumerClient<TYpKey, TOut> 
    where TYpKey: struct where TOut: CacheBaseModel
{
    Task<TOut?> GetFromCacheAsync(TYpKey key);
    Task<List<TYpKey>?> GetAllFromCacheAsync(int limit);
}