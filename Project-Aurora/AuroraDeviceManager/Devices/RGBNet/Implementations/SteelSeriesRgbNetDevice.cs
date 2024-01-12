using RGB.NET.Core;
using RGB.NET.Devices.SteelSeries;

namespace AuroraDeviceManager.Devices.RGBNet.Implementations;

public class SteelSeriesRgbNetDevice() : RgbNetDevice(true)
{
    protected override IRGBDeviceProvider Provider => SteelSeriesDeviceProvider.Instance;

    public override string DeviceName => "SteelSeries (RGB.NET)";
}