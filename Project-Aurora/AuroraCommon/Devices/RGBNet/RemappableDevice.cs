using Newtonsoft.Json;
using RGB.NET.Core;

namespace Common.Devices.RGBNet;

[JsonObject]
public class RemappableDevice
{
    public string DeviceId { get; }
    public string DeviceSummary { get; }
    
    // $"[{rgbDevice.DeviceInfo.DeviceType}] {rgbDevice.DeviceInfo.DeviceName}"
    public List<LedId> RgbNetLeds { get; }
    
    public SimpleColor Calibration { get; }

    public RemappableDevice(string deviceId, string deviceSummary, List<LedId> rgbNetLeds, SimpleColor calibration)
    {
        DeviceId = deviceId;
        DeviceSummary = deviceSummary;
        RgbNetLeds = rgbNetLeds;
        Calibration = calibration;
    }
}