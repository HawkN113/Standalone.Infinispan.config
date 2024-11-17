using System.Globalization;
using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Infinispan._14.Shared.Clients.Interfaces;
using Infinispan._14.Shared.Models;

namespace Infinispan._14.Shared.Clients;

public abstract class InfinispanClient<TIn, TYpKey, TOut>(Uri baseAddress) : IInfinispanClient<TIn, TYpKey, TOut>
    where TIn : CacheBaseModel
    where TYpKey : struct
    where TOut : CacheBaseModel
{
    private const string DefaultPath = "/rest/v2/caches";
    private const string TimeToLiveSecondsName = "TimeToLiveSeconds";

    public async Task<bool> AddToCacheAsync(TIn model, TYpKey key, string cacheName,
        NetworkCredential credentials)
    {
        var httpClient = GetClient(credentials);
        var request = new HttpRequestMessage(
            HttpMethod.Post,
            $"{DefaultPath}/{cacheName}/{key.ToString()}")
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

    public async Task<TOut?> GetFromCacheAsync(TYpKey key, string cacheName, NetworkCredential credentials)
    {
        var httpClient = GetClient(credentials);
        var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"{DefaultPath}/{cacheName}/{key.ToString()}");
        var response = await httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode) return null!;

        var content = await response.Content.ReadAsStringAsync();
        return !string.IsNullOrEmpty(content) ? JsonSerializer.Deserialize<TOut>(content) : null;
    }

    public async Task<List<TYpKey>?> GetAllFromCacheAsync(string cacheName, NetworkCredential credentials, int limit)
    {
        var httpClient = GetClient(credentials);
        
        var query = "?action=entries&content-negotiation=true&metadata=true";
        if (limit > 0)
            query += "&limit=" + limit;
        
        var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"{DefaultPath}/{cacheName}{query}");
        var response = await httpClient.SendAsync(request);
        
        if (!response.IsSuccessStatusCode) return null!;

        var content = await response.Content.ReadAsStringAsync();

        if (string.IsNullOrEmpty(content)) return null;
        
        var list = JsonSerializer.Deserialize<List<CacheEntry<TYpKey>>>(content);
        return list?.Select(s => s.Key).ToList();
    }

    public async Task<bool> DeleteFromCacheAsync(TYpKey key, string cacheName, NetworkCredential credentials)
    {
        var httpClient = GetClient(credentials);
        var request = new HttpRequestMessage(
            HttpMethod.Delete,
            $"{DefaultPath}/{cacheName}/{key.ToString()}");
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
}