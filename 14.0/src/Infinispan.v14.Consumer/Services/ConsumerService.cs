using Infinispan.v14.Consumer.Clients;
using Infinispan.v14.Shared.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Infinispan.v14.Consumer.Services;

public class ConsumerService(
    ConsumerClient client, 
    IOptions<InfinispanSettings> cacheSettings) : BackgroundService
{
    private const int DelayInSeconds = 10;

    private DateTime LastRun { get; set; } = DateTime.UtcNow;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            Console.WriteLine($"Last run was at {LastRun.ToLongTimeString()}");
            
            var list = await client.GetAllKeysFromCacheAsync(50);
            Console.WriteLine(
                $"The distributed cache '{cacheSettings.Value.CacheName}' includes total {list?.Count} entries.");
            
            foreach (var key in list!)
            {
                var cacheEntry = await client.GetFromCacheAsync(key);
                //if(cacheEntry?.CreatedUtcAt > LastRun)
                //    Console.WriteLine($"\t Cache entry: {cacheEntry?.Type} ({cacheEntry?.Manufacturer})");  
                Console.WriteLine($"\t Cache entry: {cacheEntry?.Type} ({cacheEntry?.Manufacturer}){(cacheEntry?.CreatedUtcAt > LastRun ? "-(recently added)": string.Empty)}");
            }
            LastRun = DateTime.UtcNow;
            Console.WriteLine($"Wait {DelayInSeconds} seconds...");
            await Task.Delay(DelayInSeconds * 1000, stoppingToken);
        }
    }
}