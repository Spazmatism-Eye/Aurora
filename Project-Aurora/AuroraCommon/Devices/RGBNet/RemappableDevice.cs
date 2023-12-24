using System.Text.Json.Serialization;
using RGB.NET.Core;

namespace Common.Devices.RGBNet;

public class RemappableDevice
{
    public string DeviceId { get; }
    public string DeviceSummary { get; }
    
    // $"[{rgbDevice.DeviceInfo.DeviceType}] {rgbDevice.DeviceInfo.DeviceName}"
    public List<LedId> RgbNetLeds { get; }
    
    public SimpleColor Calibration { get; }

    [JsonConstructor]
    public RemappableDevice(string deviceId, string deviceSummary, List<LedId> rgbNetLeds, SimpleColor calibration)
    {
        DeviceId = deviceId;
        DeviceSummary = deviceSummary;
        RgbNetLeds = rgbNetLeds;
        Calibration = calibration;
    }
}