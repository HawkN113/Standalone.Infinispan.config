using System.Text.Json.Serialization;
using Infinispan.v14.Shared.Models;

namespace Infinispan.v14.Consumer.Models;

public sealed class ReadableCarModel : CacheBaseModel
{
    [JsonPropertyName("manufacturer")] public string Manufacturer { get; set; }
    [JsonPropertyName("type")] public string Type { get; set; }

    [JsonPropertyName("model")] public string Model { get; set; }
    [JsonPropertyName("createdUtcAt")] public DateTime CreatedUtcAt { get; set; } = DateTime.UtcNow;
}