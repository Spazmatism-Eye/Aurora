namespace Aurora.Nodes;

public class RAMInfo : Node
{
    /// <summary>
    /// Used system memory in megabytes
    /// </summary>
    public long Used => (long)(LocalPcInformation.HardwareMonitor.Ram.RamUsed * 1024f);

    /// <summary>
    /// Free system memory in megabytes
    /// </summary>
    public long Free => (long)(LocalPcInformation.HardwareMonitor.Ram.RamFree * 1024f);

    /// <summary>
    /// Total system memory in megabytes
    /// </summary>
    public long Total => Free + Used;
}