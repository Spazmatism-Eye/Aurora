using System.Text.Json.Serialization;

namespace Common.Devices.RGBNet;

[method: JsonConstructor]
public class CurrentDevices(List<RemappableDevice> devices)
{
    public List<RemappableDevice> Devices { get; } = devices;
}