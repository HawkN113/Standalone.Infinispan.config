using System.Net;
using Infinispan.v14.Producer.Models;
using Infinispan.v14.Shared.Clients;
using Infinispan.v14.Shared.Clients.Interfaces;
using Infinispan.v14.Shared.Configuration;
using Microsoft.Extensions.Options;

namespace Infinispan.v14.Producer.Clients;

public sealed class ProducerClient(IOptions<InfinispanSettings> settings) :
    InfinispanClient<WritableCarModel, Guid>(new Uri(settings.Value.BaseAddress)),
    IProducerClient<WritableCarModel, Guid>
{
    protected override string CacheName => settings.Value.CacheName;
    
    public async Task<bool> AddToCacheAsync(WritableCarModel model, Guid key)
    {
        return await base.AddToCacheAsync(model, key, GetCredentials(AccountType.Writer));
    }

    public async Task<bool> DeleteFromCacheAsync(Guid key)
    {
        return await base.DeleteFromCacheAsync(key, GetCredentials(AccountType.Writer));
    }

    private NetworkCredential GetCredentials(AccountType accountType)
    {
        var account = settings.Value.AccessList.First(q => q.AccountType == accountType);
        return new NetworkCredential(account.Username, account.Password);
    }
}