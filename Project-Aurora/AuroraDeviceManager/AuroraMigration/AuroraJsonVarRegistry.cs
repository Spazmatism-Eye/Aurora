using System.Text.Json.Nodes;

namespace AuroraDeviceManager.AuroraMigration;

public class AuroraJsonVarRegistry(IDictionary<string, JsonNode> variables)
{
    public IDictionary<string, JsonNode> Variables { get; } = variables;
}