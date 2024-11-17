﻿using System.Net;
using Infinispan._14.Consumer.Models;
using Infinispan._14.Shared.Clients;
using Infinispan._14.Shared.Clients.Interfaces;
using Infinispan._14.Shared.Configuration;
using Microsoft.Extensions.Options;

namespace Infinispan._14.Consumer.Clients;

public sealed class ConsumerClient(IOptions<InfinispanSettings> settings) :
    InfinispanClient<ReadableCarModel, Guid, ReadableCarModel>(new Uri(settings.Value.BaseAddress)),
    IConsumerClient<Guid, ReadableCarModel>
{
    public async Task<ReadableCarModel?> GetFromCacheAsync(Guid key)
    {
        return await base.GetFromCacheAsync(key, settings.Value.CacheName, GetCredentials(AccountType.Reader));
    }

    public async Task<List<Guid>?> GetAllFromCacheAsync(int limit)
    {
        return await base.GetAllFromCacheAsync(settings.Value.CacheName, GetCredentials(AccountType.Reader), limit);
    }

    private NetworkCredential GetCredentials(AccountType accountType)
    {
        var account = settings.Value.AccessList.First(q => q.AccountType == accountType);
        return new NetworkCredential(account.Username, account.Password);
    }
}