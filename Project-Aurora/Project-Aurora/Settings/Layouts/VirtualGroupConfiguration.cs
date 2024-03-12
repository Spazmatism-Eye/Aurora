using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Common.Devices;

namespace AuroraRgb.Settings.Layouts;

public class VirtualGroupConfiguration
{
    [JsonPropertyName("keys_to_remove")]
    public DeviceKeys[] KeysToRemove { get; set; } = Array.Empty<DeviceKeys>();

    [JsonPropertyName("key_modifications")]
    public Dictionary<DeviceKeys, KeyboardKey> KeyModifications { get; set; } = new();

    [JsonPropertyName("key_conversion")]
    public Dictionary<DeviceKeys, DeviceKeys> KeyConversion { get; set; } = new();

    /// <summary>
    /// A list of paths for each included group json
    /// </summary>
    [JsonPropertyName("included_features")]
    public string[] IncludedFeatures { get; set; } = [];
}