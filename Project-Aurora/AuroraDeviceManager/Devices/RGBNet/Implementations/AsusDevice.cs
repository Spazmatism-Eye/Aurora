using AuroraDeviceManager.Utils;
using RGB.NET.Core;

namespace AuroraDeviceManager.Devices.RGBNet.Implementations;

public class AsusDevice : RgbNetDevice
{
    public override string DeviceName => "Asus (RGB.NET)";

    protected override IRGBDeviceProvider Provider => RGB.NET.Devices.Asus.AsusDeviceProvider.Instance;

    protected override Task ConfigureProvider()
    {
        base.ConfigureProvider();
        
        var isAuraRunning = ProcessUtils.IsProcessRunning("lightingservice");
        if (!isAuraRunning)
        {
            throw new DeviceProviderException(new ApplicationException("Aura or Armory Crate is not running! (LightingService.exe)"), false);
        }

        return Task.CompletedTask;
    }
}