using System.Drawing;
using Common;
using Common.Devices;

namespace AuroraDeviceManager.Devices.Omen
{
    interface IOmenDevice
    {
        public void Shutdown();
        public void SetLights(Dictionary<DeviceKeys, SimpleColor> keyColors);
        public string GetDeviceName();
    };
}
