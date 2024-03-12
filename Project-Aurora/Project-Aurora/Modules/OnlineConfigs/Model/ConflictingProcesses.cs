using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AuroraRgb.Modules.OnlineConfigs.Model;

public class ConflictingProcesses
{
    [JsonPropertyName("shutdownAuroraRgb")]
    public List<ShutdownProcess>? ShutdownAurora { get; set; }
}

public class ShutdownProcess
{
    [JsonPropertyName("processName")]
    public string ProcessName { get; set; } = "unset";

    [JsonPropertyName("reason")]
    public string? Reason { get; set; }
}