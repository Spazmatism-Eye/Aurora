using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Common;

namespace AuroraDeviceManager.AuroraMigration;

public class AuroraConfiguration(
    AuroraJsonList<string>? enabledDevices,
    AuroraJsonDictionary<SimpleColor>? deviceCalibrations,
    bool allowPeripheralDevices,
    bool devicesDisableKeyboard,
    bool devicesDisableMouse,
    bool devicesDisableHeadset,
    AuroraJsonVarRegistry? varRegistry)
{
    public AuroraJsonList<string> EnabledDevices { get; } = enabledDevices ?? new AuroraJsonList<string>([]);

    public AuroraJsonDictionary<SimpleColor> DeviceCalibrations { get; } =
        deviceCalibrations ?? new AuroraJsonDictionary<SimpleColor>(new Dictionary<string, SimpleColor>());

    [JsonPropertyName("allow_peripheral_devices")]
    public bool AllowPeripheralDevices { get; } = allowPeripheralDevices;

    [JsonPropertyName("devices_disable_keyboard")]
    public bool DevicesDisableKeyboard { get; } = devicesDisableKeyboard;

    [JsonPropertyName("devices_disable_mouse")]
    public bool DevicesDisableMouse { get; } = devicesDisableMouse;

    [JsonPropertyName("devices_disable_headset")]
    public bool DevicesDisableHeadset { get; } = devicesDisableHeadset;
    
    public AuroraJsonVarRegistry VarRegistry { get; } = varRegistry ?? new AuroraJsonVarRegistry(new Dictionary<string, JsonNode>());
}