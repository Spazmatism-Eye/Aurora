using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Aurora.Modules.OnlineConfigs.Model;

public class ConflictingProcesses
{
    [JsonPropertyName("shutdownAurora")]
    public List<ShutdownProcess>? ShutdownAurora { get; set; }
}

public class ShutdownProcess
{
    [JsonPropertyName("processName")]
    public string ProcessName { get; set; } = "unset";

    [JsonPropertyName("reason")]
    public string? Reason { get; set; }
}