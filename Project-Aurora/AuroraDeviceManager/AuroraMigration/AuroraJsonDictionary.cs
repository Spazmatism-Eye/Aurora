using System.Text.Json.Serialization;

namespace AuroraDeviceManager.AuroraMigration;

public class AuroraJsonDictionary<T>(IDictionary<string, T>? values)
{
    [JsonPropertyName("$values")]
    public IDictionary<string, T> Values { get; } = values ?? new Dictionary<string, T>();
}