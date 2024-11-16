using Infinispan._14.Shared.Clients;
using Infinispan._14.Shared.Clients.Interfaces;
using Infinispan._14.Shared.Configuration;
using Infinispan._14.Shared.Model;
using Microsoft.Extensions.Options;
using System.Net;

namespace Infinispan._14.Producer.Clients;

public sealed class ProducerClient(IOptions<InfinispanSettings> settings) :
    InfinispanClient<CarModel, Guid, CarModel>(new Uri(settings.Value.BaseAddress)),
    IProducerClient<CarModel, Guid>
{
    public async Task<bool> AddToCacheAsync(CarModel model, Guid key)
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