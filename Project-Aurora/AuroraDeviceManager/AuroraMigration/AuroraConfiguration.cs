using System.Text.Json.Serialization;
using Common;

namespace AuroraDeviceManager.AuroraMigration;

public class AuroraConfiguration(
    AuroraJsonList<string> enabledDevices,
    AuroraJsonDictionary<SimpleColor> deviceCalibrations,
    bool allowPeripheralDevices,
    bool devicesDisableKeyboard,
    bool devicesDisableMouse,
    bool devicesDisableHeadset,
    AuroraJsonVarRegistry varRegistry)
{
    public AuroraJsonList<string> EnabledDevices { get; } = enabledDevices;

    public AuroraJsonDictionary<SimpleColor> DeviceCalibrations { get; } = deviceCalibrations;

    [JsonPropertyName("allow_peripheral_devices")]
    public bool AllowPeripheralDevices { get; } = allowPeripheralDevices;

    [JsonPropertyName("devices_disable_keyboard")]
    public bool DevicesDisableKeyboard { get; } = devicesDisableKeyboard;

    [JsonPropertyName("devices_disable_mouse")]
    public bool DevicesDisableMouse { get; } = devicesDisableMouse;

    [JsonPropertyName("devices_disable_headset")]
    public bool DevicesDisableHeadset { get; } = devicesDisableHeadset;
    
    public AuroraJsonVarRegistry VarRegistry { get; } = varRegistry;
}