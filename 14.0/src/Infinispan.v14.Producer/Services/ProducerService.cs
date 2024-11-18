using Bogus;
using Infinispan.v14.Producer.Clients;
using Infinispan.v14.Producer.Models;
using Infinispan.v14.Shared.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Infinispan.v14.Producer.Services;

public class ProducerService(
    ProducerClient client, 
    IOptions<InfinispanSettings> cacheSettings) : BackgroundService
{
    private const int DelayInSeconds = 10;
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var faker = new Faker();
            for (var i = 0; i < 4; i++)
            {
                var model = new WritableCarModel()
                {
                    CacheKey = Guid.NewGuid(),
                    Model = faker.Vehicle.Model(),
                    Manufacturer = faker.Vehicle.Manufacturer(),
                    Type = faker.Vehicle.Type(),
                    TimeToLiveInSeconds = new Random().Next(-1, 80)
                };
                await client.AddToCacheAsync(model, model.CacheKey);
                Console.WriteLine(
                    $"New car model {model.Model} ({model.Manufacturer}) has been added to the distributed cache '{cacheSettings.Value.CacheName}' (expired in {model.TimeToLiveInSeconds} seconds)");
            }
            await Task.Delay((DelayInSeconds * 1000), stoppingToken);
        }
    }
}