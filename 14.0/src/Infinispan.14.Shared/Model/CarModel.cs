using System.Text.Json.Serialization;

namespace Infinispan._14.Shared.Model;

public class CarModel : CacheBaseModel
{
    [JsonPropertyName("manufacturer")] public string Manufacturer { get; set; }

    [JsonPropertyName("type")] public string Type { get; set; }

    [JsonPropertyName("model")] public string Model { get; set; }
}