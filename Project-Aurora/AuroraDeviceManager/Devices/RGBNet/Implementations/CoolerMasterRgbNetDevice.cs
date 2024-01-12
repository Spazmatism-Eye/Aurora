using RGB.NET.Core;
using RGB.NET.Devices.CoolerMaster;

namespace AuroraDeviceManager.Devices.RGBNet.Implementations;

public class CoolerMasterRgbNetDevice : RgbNetDevice
{
    protected override IRGBDeviceProvider Provider => CoolerMasterDeviceProvider.Instance;

    public override string DeviceName => "CoolerMaster (RGB.NET)";
}