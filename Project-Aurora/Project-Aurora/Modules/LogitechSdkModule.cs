using System.Threading.Tasks;
using Aurora.Modules.Logitech;
using Aurora.Modules.ProcessMonitor;
using Lombok.NET;

namespace Aurora.Modules;

public sealed class LogitechSdkModule : AuroraModule
{
    public static LogitechSdkListener LogitechSdkListener { get; } = new();

    private readonly Task<RunningProcessMonitor> _runningProcessMonitor;

    public LogitechSdkModule(Task<RunningProcessMonitor> runningProcessMonitor)
    {
        _runningProcessMonitor = runningProcessMonitor;
    }

    protected override async Task Initialize()
    {
        Global.logger.Information("Initializing Lightsync...");
        await LogitechSdkListener.Initialize(_runningProcessMonitor);
        Global.logger.Information("Initialized Lightsync");
    }

    public override Task DisposeAsync()
    {
        Dispose();
        return Task.CompletedTask;
    }

    public override void Dispose()
    {
        LogitechSdkListener.Dispose();
    }
}