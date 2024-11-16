using Bogus;
using Infinispan._14.Producer.Clients;
using Infinispan._14.Shared.Configuration;
using Infinispan._14.Shared.Model;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Infinispan._14.Producer.Services;

public class ProducerService(
    ProducerClient client, 
    IOptions<InfinispanSettings> cacheSettings) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var faker = new Faker();
            var model = new CarModel()
            {
                CacheKey = Guid.NewGuid(),
                Model = faker.Vehicle.Model(),
                Manufacturer = faker.Vehicle.Manufacturer(),
                Type = faker.Vehicle.Type(),
                TimeToLiveInSeconds = new Random().Next(20, 120)
            };
            await client.AddToCacheAsync(model, model.CacheKey);
            Console.WriteLine(
                $"New car model {model.Model} ({model.Manufacturer}) has been added to the distributed cache '{cacheSettings.Value.CacheName}'");
            await Task.Delay(10000, stoppingToken);
        }
    }
}