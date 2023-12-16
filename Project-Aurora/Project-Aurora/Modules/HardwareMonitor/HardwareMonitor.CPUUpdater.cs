using System.Collections.Generic;
using System.Linq;
using LibreHardwareMonitor.Hardware;

namespace Aurora.Modules.HardwareMonitor;

public partial class HardwareMonitor
{
    public sealed class CpuUpdater : HardwareUpdater
    {
        #region Sensors
        private readonly List<ISensor> _cpuTemp;
        public float CpuTemp => GetValue(_cpuTemp.FirstOrDefault());

        private readonly List<ISensor> _cpuLoad;
        public float CpuLoad => GetValue(_cpuLoad.FirstOrDefault());

        private readonly ISensor? _cpuPower;
        public float CpuPower => GetValue(_cpuPower);

        #endregion

        public CpuUpdater(IEnumerable<IHardware> hardware)
        {
            hw = hardware.FirstOrDefault(h => h.HardwareType == HardwareType.Cpu);
            if (hw is null)
            {
                Global.logger.Error("[HardwareMonitor] Could not find hardware of type CPU or hardware monitoring is disabled");
                _cpuTemp = new List<ISensor>();
                _cpuLoad = new List<ISensor>();
                return;
            }

            _cpuTemp = FindSensors(SensorType.Temperature);
            _cpuLoad = FindSensors(SensorType.Load);
            _cpuPower = FindSensor(SensorType.Power);

            _updateTimer.Elapsed += (_, _) =>
            {
                // To update Aurora GUI In Hardware Monitor tab
                NotifyPropertyChanged(nameof(CpuTemp));
                NotifyPropertyChanged(nameof(CpuLoad));
            };
        }
    }
}