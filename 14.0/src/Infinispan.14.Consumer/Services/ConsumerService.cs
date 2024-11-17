﻿using Infinispan._14.Consumer.Clients;
using Infinispan._14.Shared.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Infinispan._14.Consumer.Services;

public class ConsumerService(
    ConsumerClient client, 
    IOptions<InfinispanSettings> cacheSettings) : BackgroundService
{
    private const int DelayInSeconds = 10;
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var list = await client.GetAllFromCacheAsync(50);
            Console.WriteLine(
                $"The distributed cache '{cacheSettings.Value.CacheName}' includes {list.Count} entries:");

            foreach (var key in list)
            {
                var cacheEntry = await client.GetFromCacheAsync(key);
                if (cacheEntry is not null)
                    Console.WriteLine($"\t Cache entry: {cacheEntry.Type} ({cacheEntry.Manufacturer})");
            }

            Console.WriteLine($"Wait {DelayInSeconds} seconds...");
            await Task.Delay((DelayInSeconds * 1000), stoppingToken);
        }
    }
}