using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Common.Devices.RGBNet;

public class DeviceMappingConfig
{
    private static Lazy<DeviceMappingConfig> _configLoader = new(LoadConfig, LazyThreadSafetyMode.ExecutionAndPublication);
    public static DeviceMappingConfig Config => _configLoader.Value;

    [JsonIgnore]
    private static string _configPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Aurora", "DeviceMappings.json");

    [JsonPropertyName("d")]
    public List<DeviceRemap> Devices { get; init; } = new();

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
        if (!File.Exists(_configPath))
        {
            return new DeviceMappingConfig();
        }

        using var file = File.OpenText(_configPath);

        var deviceMappingConfig = JsonSerializer.Deserialize<DeviceMappingConfig>(file.ReadToEnd());
        return deviceMappingConfig ?? new DeviceMappingConfig();
    }

    public void SaveConfig()
    {
        var content = JsonSerializer.Serialize(this, new JsonSerializerOptions{ WriteIndented = true});

        Directory.CreateDirectory(Path.GetDirectoryName(_configPath) ?? throw new InvalidOperationException());
        File.WriteAllText(_configPath, content, Encoding.UTF8);
    }
}