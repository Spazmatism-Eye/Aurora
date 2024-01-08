using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Common.Utils;

namespace Common.Devices.RGBNet;

public class DeviceMappingConfig
{
    private static readonly Lazy<DeviceMappingConfig> ConfigLoader = new(LoadConfig, LazyThreadSafetyMode.ExecutionAndPublication);
    public static DeviceMappingConfig Config => ConfigLoader.Value;

    [JsonIgnore]
    private static readonly string ConfigPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Aurora", "DeviceMappings.json");

    [JsonPropertyName("d")]
    public List<DeviceRemap> Devices { get; } = [];

    private DeviceMappingConfig()
    {
    }

    [JsonConstructor]
    public DeviceMappingConfig(List<DeviceRemap> devices)
    {
        Devices = devices;
    }

    private static DeviceMappingConfig LoadConfig()
    {
        if (!File.Exists(ConfigPath))
        {
            return new DeviceMappingConfig();
        }

        var json = File.ReadAllText(ConfigPath);

        var deviceMappingConfig = JsonSerializer.Deserialize(json, CommonSourceGenerationContext.Default.DeviceMappingConfig);
        return deviceMappingConfig ?? new DeviceMappingConfig();
    }

    public void SaveConfig()
    {
        var content = JsonSerializer.Serialize(this, new JsonSerializerOptions{ WriteIndented = true});

        Directory.CreateDirectory(Path.GetDirectoryName(ConfigPath) ?? throw new InvalidOperationException());
        File.WriteAllText(ConfigPath, content, Encoding.UTF8);
    }
}