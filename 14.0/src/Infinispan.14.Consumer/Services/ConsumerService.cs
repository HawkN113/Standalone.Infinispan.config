using Infinispan._14.Consumer.Clients;
using Infinispan._14.Shared.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Infinispan._14.Consumer.Services;

public class ConsumerService(
    ConsumerClient client, 
    IOptions<InfinispanSettings> cacheSettings) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var list = await client.GetAllFromCacheAsync();
            Console.WriteLine(
                $"The distributed cache '{cacheSettings.Value.CacheName}' includes {list.Count} entries:");

            foreach (var key in list)
            {
                var cacheEntry = await client.GetFromCacheAsync(key);
                Console.WriteLine(
                    $"\t{cacheEntry.Type} ({cacheEntry.Manufacturer})");
            }
            
            
            await Task.Delay(15000, stoppingToken);
        }
    }
}