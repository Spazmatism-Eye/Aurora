using System.Text.Json.Serialization;
using Common.Devices;
using Common.Devices.RGBNet;

namespace Common.Utils;

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(DeviceMappingConfig))]
[JsonSerializable(typeof(DeviceConfig))]
public partial class CommonSourceGenerationContext : JsonSerializerContext
{
    
}