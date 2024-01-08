using System.Text.Json.Serialization;
using RGB.NET.Core;

namespace Common.Devices.RGBNet;

[method: JsonConstructor]
public class RemappableDevice(string deviceId, string deviceSummary, List<LedId> rgbNetLeds, SimpleColor calibration, bool remapEnabled)
{
    public string DeviceId { get; } = deviceId;
    public string DeviceSummary { get; } = deviceSummary;

    // $"[{rgbDevice.DeviceInfo.DeviceType}] {rgbDevice.DeviceInfo.DeviceName}"
    public List<LedId> RgbNetLeds { get; } = rgbNetLeds;

    public SimpleColor Calibration { get; } = calibration;

    public bool RemapEnabled { get; } = remapEnabled;
}