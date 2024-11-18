using Infinispan.v14.Monitor.Clients;
using Infinispan.v14.Shared.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Infinispan.v14.Monitor.Services;

public class MonitorService(
    MonitorClient client, 
    IOptions<InfinispanSettings> cacheSettings) : BackgroundService
{
    private const int DelayInSeconds = 5;
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            Console.SetCursorPosition(0, 0);
            var result = await client.GetStatisticsAsync();
            if (result is not null)
            {
                Console.WriteLine($"Statistics of the cache '{cacheSettings.Value.CacheName}':");
                Console.WriteLine($"Total number:\t{result.Stats.TotalUniqueNumberOfEntries}");
                Console.WriteLine($"Stores:\t\t{result.Stats.Stores}");
                Console.WriteLine($"Hits:\t\t{result.Stats.Hits}");
                Console.WriteLine($"Misses:\t\t{result.Stats.Misses}");
                Console.WriteLine($"Retrievals:\t{result.Stats.Retrievals}");
                Console.WriteLine($"IsQueryable:\t{result.IsQueryable}");
                Console.WriteLine($"Key type:\t{result.KeyStorageType}");
                Console.WriteLine($"Value type:\t{result.ValueStorageType}");
            }
            await Task.Delay(DelayInSeconds * 1000, stoppingToken);
        }
    }
}