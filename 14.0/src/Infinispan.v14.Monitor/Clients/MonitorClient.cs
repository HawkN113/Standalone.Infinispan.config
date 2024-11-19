using System.Net;
using Infinispan.v14.Shared.Clients;
using Infinispan.v14.Shared.Clients.Interfaces;
using Infinispan.v14.Shared.Configuration;
using Infinispan.v14.Shared.Models;
using Microsoft.Extensions.Options;

namespace Infinispan.v14.Monitor.Clients;

public sealed class MonitorClient(IOptions<InfinispanSettings> settings) :
    CacheMonitorClient(new Uri(settings.Value.BaseAddress)),
    IMonitorClient
{
    protected override string CacheMonitorName => settings.Value.CacheName;

    public async Task<StatsModel?> GetStatisticsAsync()
    {
        return await base.GetStatisticsAsync(GetCredentials(AccountType.Monitor));
    }
    private NetworkCredential GetCredentials(AccountType accountType)
    {
        var account = settings.Value.AccessList.First(q => q.AccountType == accountType);
        return new NetworkCredential(account.Username, account.Password);
    }
}