using System.Collections.Generic;
using System.Text.Json.Serialization;
using Common.Devices;

namespace Aurora.Settings.Layouts;

internal sealed class KeyboardLayout(Dictionary<DeviceKeys, DeviceKeys>? keyConversion, KeyboardKey[]? keys)
{
    [JsonPropertyName("key_conversion")]
    public Dictionary<DeviceKeys, DeviceKeys> KeyConversion { get; } = keyConversion ?? new Dictionary<DeviceKeys, DeviceKeys>();

    [JsonPropertyName("keys")]
    public KeyboardKey[] Keys { get; } = keys ?? [];
}