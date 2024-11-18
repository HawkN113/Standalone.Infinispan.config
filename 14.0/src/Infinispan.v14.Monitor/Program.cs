using Infinispan.v14.Monitor.Clients;
using Infinispan.v14.Monitor.Services;
using Infinispan.v14.Shared.Extensions;
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
        services.AddScoped<MonitorClient>();
        // Add background services
        services.AddHostedService<MonitorService>();
    })
    .Build();

try
{
    await host.RunAsync();
}
catch (Exception ex)
{
    Console.WriteLine($"Fatal error: {ex.Message}, {ex.StackTrace}");
}