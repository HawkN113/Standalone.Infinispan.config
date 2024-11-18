using System.Text.Json.Serialization;

namespace Infinispan.v14.Shared.Models;

public class DemoModel : CacheBaseModel
{
    [JsonPropertyName("message")]
    public string? Message { get; set; }
    
}