using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using Infinispan.v14.Shared.Clients.Abstraction;
using Infinispan.v14.Shared.Models;

namespace Infinispan.v14.Shared.Clients;

public abstract class CacheReaderClient<T, TYpKey>(Uri baseAddress) : ICacheReaderClient<T, TYpKey>
    where T : CacheBaseModel, new()
    where TYpKey : struct
{
    private const string DefaultPath = "/rest/v2/caches";
    protected abstract string CacheReaderName { get; }

    public async Task<T?> GetFromCacheAsync(TYpKey key, NetworkCredential credentials)
    {
        var httpClient = Helper.GetClient(credentials, baseAddress);
        var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"{DefaultPath}/{CacheReaderName}/{key.ToString()}");
        var response = await httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode) return null!;

        var content = await response.Content.ReadAsStringAsync();
        return !string.IsNullOrEmpty(content) ? JsonSerializer.Deserialize<T>(content) : null;
    }

    public async Task<List<TYpKey>?> GetAllKeysFromCacheAsync(NetworkCredential credentials, int limit)
    {
        var httpClient = Helper.GetClient(credentials, baseAddress);
        
        var queryString = "?action=entries&content-negotiation=false&metadata=false";
        if (limit > 0)
            queryString += "&limit=" + limit;
        
        var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"{DefaultPath}/{CacheReaderName}{queryString}");
        var response = await httpClient.SendAsync(request);
        
        if (!response.IsSuccessStatusCode) return null!;

        var content = await response.Content.ReadAsStringAsync();

        if (string.IsNullOrEmpty(content)) return null;
        
        var list = JsonSerializer.Deserialize<List<CacheEntry<TYpKey>>>(content);
        return list?.Select(s => s.Key).ToList();
    }

    public async Task<List<T>> GetByQueryFromCacheAsync(Func<T, bool> query, NetworkCredential credentials,
        int limit)
    {
        var httpClient = Helper.GetClient(credentials, baseAddress);

        const string queryString = "?action=entries&content-negotiation=false&metadata=false";

        var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"{DefaultPath}/{CacheReaderName}{queryString}");
        var response = await httpClient.SendAsync(request);

        var content = await response.Content.ReadAsStringAsync();

        if (string.IsNullOrEmpty(content)) return [];
        
        var cacheEntries = JsonSerializer.Deserialize<List<CacheEntry<TYpKey>>>(content);

        return cacheEntries?
            .AsParallel()
            .Select(entry =>
            {
                var value = entry.Value is not null
                    ? JsonSerializer.Deserialize<T>(entry.Value.ToString()!)
                    : null;
                if (value is not null && Guid.TryParse(entry.Key.ToString(), out var key))
                    value.CacheKey = key;
                return value;
            })
            .Where(item => item is not null && query(item))
            .Take(limit)
            .ToList()!;
    }
}

internal class CacheEntry<TYpKey> where TYpKey : struct 
{
    [JsonPropertyName("key")]
    public TYpKey Key { get; set; }
    
    [JsonPropertyName("value")]
    public object? Value { get; set; }
}