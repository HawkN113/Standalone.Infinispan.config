using System.Globalization;
using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using Infinispan.v14.Shared.Clients.Interfaces;
using Infinispan.v14.Shared.Models;

namespace Infinispan.v14.Shared.Clients;

public abstract class CacheWriterClient<T, TYpKey>(Uri baseAddress) : ICacheWriterClient<T, TYpKey>
    where T : CacheBaseModel, new()
    where TYpKey : struct
{
    private const string DefaultPath = "/rest/v2/caches";
    private const string TimeToLiveSecondsName = "TimeToLiveSeconds";
    protected abstract string CacheWriterName { get; }

    public async Task<bool> AddToCacheAsync(T model, TYpKey key,
        NetworkCredential credentials)
    {
        var httpClient = Helper.GetClient(credentials, baseAddress);
        var request = new HttpRequestMessage(
            HttpMethod.Post,
            $"{DefaultPath}/{CacheWriterName}/{key.ToString()}")
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
    
    public virtual async Task<bool> DeleteFromCacheAsync(TYpKey key, NetworkCredential credentials)
    {
        var httpClient = Helper.GetClient(credentials, baseAddress);
        var request = new HttpRequestMessage(
            HttpMethod.Delete,
            $"{DefaultPath}/{CacheWriterName}/{key.ToString()}");
        var response = await httpClient.SendAsync(request);
        if (response.IsSuccessStatusCode) return response.IsSuccessStatusCode;
        var errorContent = await response.Content.ReadAsStringAsync();
        throw new ArgumentNullException($"DeleteFromCacheAsync: {errorContent}");
    }
}