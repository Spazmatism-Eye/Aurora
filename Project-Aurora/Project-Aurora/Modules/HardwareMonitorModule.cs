using System.Threading.Tasks;
using Aurora.Profiles;
using Lombok.NET;
using Aurora.Modules.HardwareMonitor;
using Aurora.Nodes;

namespace Aurora.Modules;

public sealed partial class HardwareMonitorModule : AuroraModule
{
    protected override Task Initialize()
    {
        if (Global.Configuration.EnableHardwareInfo)
        {
            LocalPcInformation.HardwareMonitor = new HardwareMonitor.HardwareMonitor();
        }

        return Task.CompletedTask;
    }


    [Async]
    public override void Dispose()
    {
        LocalPcInformation.HardwareMonitor = new NoopHardwareMonitor();
    }
}