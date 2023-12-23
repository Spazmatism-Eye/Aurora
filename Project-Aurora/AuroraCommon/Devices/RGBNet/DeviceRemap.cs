using System.Text.Json.Serialization;
using RGB.NET.Core;

namespace Common.Devices.RGBNet;

public class DeviceRemap
{
    [JsonPropertyName("n")]
    public string Name { get; }

    [JsonPropertyName("k")]
    public Dictionary<LedId, DeviceKeys> KeyMapper { get; } = new(Constants.MaxKeyId);

    [JsonConstructor]
    public DeviceRemap(string name, Dictionary<LedId, DeviceKeys> keyMapper)
    {
        Name = name;
        KeyMapper = keyMapper;
    }

    public DeviceRemap(string name)
    {
        Name = name;
    }

    public DeviceRemap(RemappableDevice device)
    {
        Name = device.DeviceId;
    }

    public override string ToString()
    {
        return Name;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not DeviceRemap device)
            return false;

        return Name == device.Name;
    }

    public override int GetHashCode()
    {
        return Name.GetHashCode();
    }
}