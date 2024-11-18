using System.Net;
using Infinispan.v14.Consumer.Models;
using Infinispan.v14.Shared.Clients;
using Infinispan.v14.Shared.Clients.Interfaces;
using Infinispan.v14.Shared.Configuration;
using Microsoft.Extensions.Options;

namespace Infinispan.v14.Consumer.Clients;

public sealed class ConsumerClient(IOptions<InfinispanSettings> settings) :
    InfinispanClient<ReadableCarModel, Guid>(new Uri(settings.Value.BaseAddress)),
    IConsumerClient<Guid, ReadableCarModel>
{
    protected override string CacheName => settings.Value.CacheName;

    public async Task<ReadableCarModel?> GetFromCacheAsync(Guid key)
    {
        return await base.GetFromCacheAsync(key, GetCredentials(AccountType.Reader));
    }

    public async Task<List<Guid>?> GetAllKeysFromCacheAsync(int limit)
    {
        return await base.GetAllKeysFromCacheAsync(GetCredentials(AccountType.Reader), limit);
    }

    public async Task<IEnumerable<ReadableCarModel>> GetByQueryFromCacheAsync(Func<ReadableCarModel, bool> query,
        int limit)
    {
        return await base.GetByQueryFromCacheAsync(query, GetCredentials(AccountType.Reader), limit);
    }

    private NetworkCredential GetCredentials(AccountType accountType)
    {
        var account = settings.Value.AccessList.First(q => q.AccountType == accountType);
        return new NetworkCredential(account.Username, account.Password);
    }
}