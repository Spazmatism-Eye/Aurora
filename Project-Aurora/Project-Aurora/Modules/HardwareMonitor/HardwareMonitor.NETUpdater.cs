using System.Collections.Generic;
using System.Linq;
using LibreHardwareMonitor.Hardware;

namespace Aurora.Modules.HardwareMonitor;

public partial class HardwareMonitor
{
    public sealed class NetUpdater : HardwareUpdater
    {
        #region Sensors
        private readonly ISensor? _bandwidthUsed;
        public float BandwidthUsed => GetValue(_bandwidthUsed);

        private readonly ISensor? _uploadSpeed;
        public float UploadSpeedBytes => GetValue(_uploadSpeed);

        private readonly ISensor? _downloadSpeed;
        public float DownloadSpeedBytes => GetValue(_downloadSpeed);
        #endregion

        public NetUpdater(IEnumerable<IHardware> hardware)
        {
            hw = hardware.FirstOrDefault(w => w.HardwareType == HardwareType.Network);
            if (hw is null)
            {
                Global.logger.Error("[HardwareMonitor] Could not find hardware of type Network or hardware monitoring is disabled");
                return;
            }
            _bandwidthUsed = FindSensor(SensorType.Load);
            _uploadSpeed = FindSensor("throughput/7");
            _downloadSpeed = FindSensor("throughput/8");
        }
    }
}