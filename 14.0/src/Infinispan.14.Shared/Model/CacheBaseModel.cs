﻿using System.Text.Json.Serialization;

namespace Infinispan._14.Shared.Model;

public abstract class CacheBaseModel
{
    [JsonIgnore]
    public Guid CacheKey { get; set; }
    
    [JsonIgnore]
    public int TimeToLiveInSeconds { get; set; } = -1;
}