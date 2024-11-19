using System.Globalization;
using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Infinispan.v14.Shared.Clients.Interfaces;
using Infinispan.v14.Shared.Models;

namespace Infinispan.v14.Shared.Clients;

public abstract class InfinispanClient<T, TYpKey>(Uri baseAddress) : IInfinispanClient<T, TYpKey>
    where T : CacheBaseModel, new()
    where TYpKey : struct
{
    private const string DefaultPath = "/rest/v2/caches";
    private const string TimeToLiveSecondsName = "TimeToLiveSeconds";
    protected abstract string CacheName { get; }

    public virtual async Task<bool> AddToCacheAsync(T model, TYpKey key,
        NetworkCredential credentials)
    {
        var httpClient = GetClient(credentials);
        var request = new HttpRequestMessage(
            HttpMethod.Post,
            $"{DefaultPath}/{CacheName}/{key.ToString()}")
        {
            Content = new StringContent(
                JsonSerializer.Serialize(model),
                Encoding.UTF8,
                MediaTypeNames.Application.Json)
        };
        if (model.TimeToLiveInSeconds > 0)
            request.Content.Headers.Add(TimeToLiveSecondsName,
                model.TimeToLiveInSeconds.ToString(CultureInfo.InvariantCulture));
        var response = await httpClient.SendAsync(request);
        if (response.IsSuccessStatusCode) return response.IsSuccessStatusCode;
        var errorContent = await response.Content.ReadAsStringAsync();
        throw new ArgumentNullException($"AddToCacheAsync: {errorContent}");
    }

    public virtual async Task<T?> GetFromCacheAsync(TYpKey key, NetworkCredential credentials)
    {
        var httpClient = GetClient(credentials);
        var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"{DefaultPath}/{CacheName}/{key.ToString()}");
        var response = await httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode) return null!;

        var content = await response.Content.ReadAsStringAsync();
        return !string.IsNullOrEmpty(content) ? JsonSerializer.Deserialize<T>(content) : null;
    }

    public virtual async Task<List<TYpKey>?> GetAllKeysFromCacheAsync(NetworkCredential credentials, int limit)
    {
        var httpClient = GetClient(credentials);
        
        var queryString = "?action=entries&content-negotiation=false&metadata=false";
        if (limit > 0)
            queryString += "&limit=" + limit;
        
        var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"{DefaultPath}/{CacheName}{queryString}");
        var response = await httpClient.SendAsync(request);
        
        if (!response.IsSuccessStatusCode) return null!;

        var content = await response.Content.ReadAsStringAsync();

        if (string.IsNullOrEmpty(content)) return null;
        
        var list = JsonSerializer.Deserialize<List<CacheEntry<TYpKey>>>(content);
        return list?.Select(s => s.Key).ToList();
    }

    public virtual async Task<List<T>> GetByQueryFromCacheAsync(Func<T, bool> query, NetworkCredential credentials,
        int limit)
    {
        var httpClient = GetClient(credentials);

        const string queryString = "?action=entries&content-negotiation=false&metadata=false";

        var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"{DefaultPath}/{CacheName}{queryString}");
        var response = await httpClient.SendAsync(request);

        var content = await response.Content.ReadAsStringAsync();

        if (string.IsNullOrEmpty(content)) return [];
        var cacheEntries = JsonSerializer.Deserialize<List<CacheEntry<TYpKey>>>(content);

        return cacheEntries?
            .AsParallel()
            .Select(entry =>
            {
                var value = entry.Value is not null
                    ? JsonSerializer.Deserialize<T>(entry.Value.ToString())
                    : null;
                if (value is not null && Guid.TryParse(entry.Key.ToString(), out var key))
                    value.CacheKey = key;
                return value;
            })
            .Where(item => item is not null && query(item))
            .Take(limit)
            .ToList();
    }

    public virtual async Task<StatsModel?> GetStatisticsAsync(NetworkCredential credentials)
    {
        var httpClient = GetClient(credentials);
        var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"{DefaultPath}/{CacheName}");
        var response = await httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode) return null!;

        var content = await response.Content.ReadAsStringAsync();
        return !string.IsNullOrEmpty(content) ? JsonSerializer.Deserialize<StatsModel>(content) : null;
    }

    public virtual async Task<bool> DeleteFromCacheAsync(TYpKey key, NetworkCredential credentials)
    {
        var httpClient = GetClient(credentials);
        var request = new HttpRequestMessage(
            HttpMethod.Delete,
            $"{DefaultPath}/{CacheName}/{key.ToString()}");
        var response = await httpClient.SendAsync(request);
        if (response.IsSuccessStatusCode) return response.IsSuccessStatusCode;
        var errorContent = await response.Content.ReadAsStringAsync();
        throw new ArgumentNullException($"DeleteFromCacheAsync: {errorContent}");
    }

    private HttpClient GetClient(NetworkCredential credentials)
    {
        var handler = new HttpClientHandler
        {
            Credentials = credentials
        };
        var httpClient = new HttpClient(handler)
        {
            BaseAddress = baseAddress
        };
        return httpClient;
    }
}

internal class CacheEntry<TYpKey> where TYpKey : struct 
{
    [JsonPropertyName("key")]
    public TYpKey Key { get; set; }
    
    [JsonPropertyName("value")]
    public object? Value { get; set; }
}