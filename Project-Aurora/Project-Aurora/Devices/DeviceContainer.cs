using System;
using System.Threading.Tasks;

namespace Aurora.Devices;

public sealed class DeviceContainer(MemorySharedDevice device, DeviceManager deviceManager) : IDisposable
{
    public MemorySharedDevice Device { get; } = device;

    public async Task EnableDevice()
    {
        await deviceManager.DevicesPipe.EnableDevice(Device.DeviceName);
    }

    public async Task DisableDevice()
    {
        await deviceManager.DevicesPipe.DisableDevice(Device.DeviceName);
    }

    public void Dispose()
    {
        Device.Dispose();
    }
}