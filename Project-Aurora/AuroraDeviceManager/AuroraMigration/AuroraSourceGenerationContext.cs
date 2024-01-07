using System.Text.Json.Serialization;

namespace AuroraDeviceManager.AuroraMigration;

[JsonSerializable(typeof(AuroraJsonList<string>))]
[JsonSerializable(typeof(AuroraConfiguration))]
public partial class AuroraSourceGenerationContext : JsonSerializerContext;