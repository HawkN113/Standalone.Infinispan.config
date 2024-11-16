using System.Net;
using Infinispan._14.Shared.Clients;
using Infinispan._14.Shared.Clients.Interfaces;
using Infinispan._14.Shared.Configuration;
using Infinispan._14.Shared.Model;
using Microsoft.Extensions.Options;

namespace Infinispan._14.Consumer.Clients;

public sealed class ConsumerClient(IOptions<InfinispanSettings> settings) :
    InfinispanClient<CarModel, Guid, CarModel>(new Uri(settings.Value.BaseAddress)),
    IConsumerClient<Guid, CarModel>
{
    public async Task<CarModel?> GetFromCacheAsync(Guid key)
    {
        return await base.GetFromCacheAsync(key, settings.Value.CacheName, GetCredentials(AccountType.Reader));
    }

    public async Task<List<Guid>?> GetAllFromCacheAsync()
    {
        return await base.GetAllFromCacheAsync(settings.Value.CacheName, GetCredentials(AccountType.Reader));
    }

    private NetworkCredential GetCredentials(AccountType accountType)
    {
        var account = settings.Value.AccessList.First(q => q.AccountType == accountType);
        return new NetworkCredential(account.Username, account.Password);
    }
}