using System.Text.Json.Serialization;

namespace Common.Devices.RGBNet;

public class CurrentDevices
{
    [JsonConstructor]
    public CurrentDevices(List<RemappableDevice> devices)
    {
        Devices = devices;
    }

    public List<RemappableDevice> Devices { get; }
}