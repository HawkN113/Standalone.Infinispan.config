using Infinispan.v14.Shared.Configuration;
using Infinispan.v14.Consumer.Clients;
using Infinispan.v14.Consumer.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(context =>
    {
        context.AddJsonFile("appsettings.Development.json", false);
    })
    .ConfigureServices((context, services)  =>
    {
        // Add cache settings
        services.AddCacheSettings(context.Configuration);
        // Add clients
        services.AddScoped<ConsumerClient>();
        // Add background services
        services.AddHostedService<ConsumerService>();
    })
    .Build();

try
{
    Console.WriteLine("Start background services...");
    await host.RunAsync();
}
catch (Exception ex)
{
    Console.WriteLine($"Fatal error: {ex.Message}, {ex.StackTrace}");
}