using System.Net;
using Infinispan.v14.Producer.Models;
using Infinispan.v14.Shared.Clients;
using Infinispan.v14.Shared.Clients.Interfaces;
using Infinispan.v14.Shared.Configuration;
using Microsoft.Extensions.Options;

namespace Infinispan.v14.Producer.Clients;

public sealed class ProducerClient(IOptions<InfinispanSettings> settings) :
    InfinispanClient<WritableCarModel, Guid, WritableCarModel>(new Uri(settings.Value.BaseAddress)),
    IProducerClient<WritableCarModel, Guid>
{
    public async Task<bool> AddToCacheAsync(WritableCarModel model, Guid key)
    {
        return await base.AddToCacheAsync(model, key, settings.Value.CacheName, GetCredentials(AccountType.Writer));
    }

    public async Task<bool> DeleteFromCacheAsync(Guid key)
    {
        return await base.DeleteFromCacheAsync(key, settings.Value.CacheName, GetCredentials(AccountType.Writer));
    }

    private NetworkCredential GetCredentials(AccountType accountType)
    {
        var account = settings.Value.AccessList.First(q => q.AccountType == accountType);
        return new NetworkCredential(account.Username, account.Password);
    }
}