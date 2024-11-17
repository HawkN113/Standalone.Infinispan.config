using System.Text.Json.Serialization;

namespace Infinispan._14.Shared.Models;

public class DemoModel : CacheBaseModel
{
    [JsonPropertyName("message")]
    public string? Message { get; set; }
    
}