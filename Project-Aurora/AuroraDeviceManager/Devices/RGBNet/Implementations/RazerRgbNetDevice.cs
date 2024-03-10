using Common.Devices;
using RGB.NET.Devices.Razer;

namespace AuroraDeviceManager.Devices.RGBNet.Implementations;

public class RazerRgbNetDevice : RgbNetDevice
{
    protected override RazerDeviceProvider Provider => RazerDeviceProvider.Instance;

    public override string DeviceName => "Razer (RGB.NET)";

    protected override async Task ConfigureProvider(CancellationToken cancellationToken)
    {
        await base.ConfigureProvider(cancellationToken);
        
        Provider.LoadEmulatorDevices = Global.DeviceConfig.VarRegistry.GetVariable<bool>($"{DeviceName}_force_all");
    }

    protected override void RegisterVariables(VariableRegistry variableRegistry)
    {
        base.RegisterVariables(variableRegistry);
        
        variableRegistry.Register($"{DeviceName}_force_all", false, "Force enable all devices");
    }
}