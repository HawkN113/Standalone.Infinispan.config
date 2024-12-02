using System.Text.Json.Serialization;

namespace Infinispan.v14.Shared.Models;

public class StatsModel: CacheBaseModel
{
    [JsonPropertyName("stats")]
    public required Stats Stats { get; set; }
    
    [JsonPropertyName("key_storage")]
    public required string KeyStorageType { get; set; }
    
    [JsonPropertyName("value_storage")]
    public required string ValueStorageType { get; set; }
    
    [JsonPropertyName("queryable")]
    public required bool IsQueryable { get; set; }
}

public abstract class Stats
{
    [JsonPropertyName("approximate_entries_unique")]
    public required int TotalUniqueNumberOfEntries { get; set; }
    
    [JsonPropertyName("stores")]
    public required int Stores { get; set; }
    
    [JsonPropertyName("retrievals")]
    public required int Retrievals { get; set; }
    
    [JsonPropertyName("hits")]
    public required int Hits { get; set; }
    
    [JsonPropertyName("misses")]
    public required int Misses { get; set; }
}