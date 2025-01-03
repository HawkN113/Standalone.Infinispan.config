using System.Net;
using System.Text.Json;
using Infinispan.v14.Shared.Clients.Abstraction;
using Infinispan.v14.Shared.Models;

namespace Infinispan.v14.Shared.Clients;

public abstract class CacheMonitorClient(Uri baseAddress) : ICacheMonitorClient
{
    private const string DefaultPath = "/rest/v2/caches";
    protected abstract string CacheMonitorName { get; }

    public async Task<StatsModel?> GetStatisticsAsync(NetworkCredential credentials)
    {
        var httpClient = Helper.GetClient(credentials, baseAddress);
        var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"{DefaultPath}/{CacheMonitorName}");
        var response = await httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode) return null!;

        var content = await response.Content.ReadAsStringAsync();
        return !string.IsNullOrEmpty(content) ? JsonSerializer.Deserialize<StatsModel>(content) : null;
    }
}