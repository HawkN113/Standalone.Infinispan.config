using Infinispan.v14.Shared.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infinispan.v14.Shared.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCacheSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<InfinispanSettings>(configuration.GetSection("InfinispanSettings"));
        return services;
    }
}