using System.Text.Json.Serialization;

namespace Aurora.Modules.OnlineConfigs.Model;

public class DeviceTooltips
{
    [JsonPropertyName("recommended")]
    public bool Recommended { get; }

    [JsonPropertyName("beta")]
    public bool Beta { get; }

    [JsonPropertyName("info")]
    public string? Info { get; }

    [JsonPropertyName("sdkLink")]
    public string? SdkLink { get; }

    public DeviceTooltips()
    {
    }

    [JsonConstructor]
    public DeviceTooltips(bool recommended, bool beta, string? info, string? sdkLink)
    {
        Recommended = recommended;
        Beta = beta;
        Info = info;
        SdkLink = sdkLink;
    }
}