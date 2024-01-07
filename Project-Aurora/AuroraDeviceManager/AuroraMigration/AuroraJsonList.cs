using System.Text.Json.Serialization;

namespace AuroraDeviceManager.AuroraMigration;

public class AuroraJsonList<T>(IList<T>? values)
{
    [JsonPropertyName("$values")]
    public IList<T> Values { get; } = values ?? [];
}