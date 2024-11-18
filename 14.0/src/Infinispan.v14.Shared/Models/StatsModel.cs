using System.Text.Json.Serialization;

namespace Infinispan.v14.Shared.Models;

public class StatsModel: CacheBaseModel
{
    [JsonPropertyName("stats")]
    public Stats Stats { get; set; }
    
    [JsonPropertyName("key_storage")]
    public string KeyStorageType { get; set; }
    
    [JsonPropertyName("value_storage")]
    public string ValueStorageType { get; set; }
    
    [JsonPropertyName("queryable")]
    public bool IsQueryable { get; set; }
}

public class Stats
{
    [JsonPropertyName("approximate_entries_unique")]
    public int TotalUniqueNumberOfEntries { get; set; }
    
    [JsonPropertyName("stores")]
    public int Stores { get; set; }
    
    [JsonPropertyName("retrievals")]
    public int Retrievals { get; set; }
    
    [JsonPropertyName("hits")]
    public int Hits { get; set; }
    
    [JsonPropertyName("misses")]
    public int Misses { get; set; }
}