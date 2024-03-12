using System.Collections.Generic;
using System.Linq;
using LibreHardwareMonitor.Hardware;

namespace AuroraRgb.Modules.HardwareMonitor;

public partial class HardwareMonitor
{
    public sealed class GpuUpdater : HardwareUpdater
    {
        #region Sensors
        private readonly ISensor? _gpuTemp;
        public float GpuCoreTemp => GetValue(_gpuTemp);

        private readonly ISensor? _gpuFan;
        public float GpuFan => GetValue(_gpuFan);

        private readonly ISensor? _gpuLoad;
        public float GpuLoad => GetValue(_gpuLoad);

        private readonly ISensor? _gpuPower;
        public float GpuPower => GetValue(_gpuPower);
        #endregion

        public GpuUpdater(IEnumerable<IHardware> hardware)
        {
            hw = hardware.FirstOrDefault(h => h.HardwareType is HardwareType.GpuAmd or HardwareType.GpuNvidia);
            if (hw is null)
            {
                Global.logger.Error("[HardwareMonitor] Could not find hardware of type GPU or hardware monitoring is disabled");
                return;
            }
            _gpuLoad = FindSensor(SensorType.Load);
            _gpuTemp = FindSensor(SensorType.Temperature);
            _gpuFan = FindSensor(SensorType.Fan);
            _gpuPower = FindSensor(SensorType.Power);
        }
    }
}