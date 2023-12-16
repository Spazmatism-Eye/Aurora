using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using LibreHardwareMonitor.Hardware;

namespace Aurora.Modules.HardwareMonitor;

public interface IHardwareMonitor
{
    HardwareMonitor.GpuUpdater Gpu { get; }
    HardwareMonitor.CpuUpdater Cpu { get; }
    HardwareMonitor.RamUpdater Ram { get; }
    HardwareMonitor.NetUpdater Net { get; }
}

public class NoopHardwareMonitor : IHardwareMonitor
{
    private readonly Lazy<HardwareMonitor.GpuUpdater> _gpu = new(() => new (new List<IHardware>()));
    private readonly Lazy<HardwareMonitor.CpuUpdater> _cpu = new(() => new (new List<IHardware>()));
    private readonly Lazy<HardwareMonitor.NetUpdater> _net = new(() => new (new List<IHardware>()));
    private readonly Lazy<HardwareMonitor.RamUpdater> _ram = new(() => new (new List<IHardware>()));

    public HardwareMonitor.GpuUpdater Gpu => _gpu.Value;

    public HardwareMonitor.CpuUpdater Cpu => _cpu.Value;

    public HardwareMonitor.RamUpdater Ram => _ram.Value;

    public HardwareMonitor.NetUpdater Net => _net.Value;
}

public partial class HardwareMonitor: IHardwareMonitor
{
    private static readonly IEnumerable<IHardware> Hardware;

    private static readonly Lazy<GpuUpdater> _gpu = new(() => new GpuUpdater(Hardware), LazyThreadSafetyMode.ExecutionAndPublication);
    public GpuUpdater Gpu => _gpu.Value;

    private static readonly Lazy<CpuUpdater> _cpu = new(() => new CpuUpdater(Hardware), LazyThreadSafetyMode.ExecutionAndPublication);
    public CpuUpdater Cpu => _cpu.Value;

    private static readonly Lazy<RamUpdater> _ram = new(() => new RamUpdater(Hardware), LazyThreadSafetyMode.ExecutionAndPublication);
    public RamUpdater Ram => _ram.Value;

    private static readonly Lazy<NetUpdater> _net = new(() => new NetUpdater(Hardware), LazyThreadSafetyMode.ExecutionAndPublication);
    public NetUpdater Net => _net.Value;

#pragma warning disable CA1810 // Initialize reference type static fields inline
    static HardwareMonitor()
#pragma warning restore CA1810 // Initialize reference type static fields inline
    {
        var computer = new Computer
        {
            IsCpuEnabled = true,
            IsGpuEnabled = true,
            IsMemoryEnabled = true,
            IsNetworkEnabled = true
        };
        try
        {
            computer.Open();
            Hardware = computer.Hardware;
        }
        catch (Exception e)
        {
            Global.logger.Error("Error instantiating hardware monitor:", e);
        }
    }

    public static bool TryDump()
    {
        var lines = new List<string>();
        foreach (var hw in Hardware)
        {
            lines.Add("-----");
            lines.Add(hw.Name);
            lines.Add("Sensors:");
            lines.AddRange(
                hw.Sensors.OrderBy(s => s.Identifier)
                    .Select(sensor => $"Name: {sensor.Name}, Id: {sensor.Identifier}, Type: {sensor.SensorType}"));
            lines.Add("-----");
        }
        try
        {
            File.WriteAllLines(Path.Combine(Global.LogsDirectory, "sensors.txt"), lines);
            return true;
        }
        catch (IOException e)
        {
            Global.logger.Error(e, "Failed to write sensors dump");
            return false;
        }
    }
}