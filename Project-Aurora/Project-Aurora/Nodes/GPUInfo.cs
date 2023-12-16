namespace Aurora.Nodes;

public class GPUInfo : Node
{
    public float Usage => LocalPcInformation.HardwareMonitor.Gpu.GpuLoad;
    public float Temperature => LocalPcInformation.HardwareMonitor.Gpu.GpuCoreTemp;
    public float PowerUsage => LocalPcInformation.HardwareMonitor.Gpu.GpuPower;
    public float FanRPM => LocalPcInformation.HardwareMonitor.Gpu.GpuFan;
}