using Infinispan.v14.Producer.Clients;
using Infinispan.v14.Producer.Services;
using Infinispan.v14.Shared.Configuration;
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
        services.Configure<InfinispanSettings>(context.Configuration.GetSection("InfinispanSettings"));
        // Add clients
        services.AddScoped<ProducerClient>();
        // Add background services
        services.AddHostedService<ProducerService>();
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