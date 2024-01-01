using System.Text.Json.Serialization;
using Common.Devices.RGBNet;

namespace Common.Utils;

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(DeviceMappingConfig))]
public partial class CommonSourceGenerationContext : JsonSerializerContext
{
    
}