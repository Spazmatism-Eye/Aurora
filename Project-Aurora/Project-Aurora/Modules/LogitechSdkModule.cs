using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Aurora.Modules.Logitech;
using Aurora.Modules.ProcessMonitor;

namespace Aurora.Modules;

public sealed class LogitechSdkModule(Task<RunningProcessMonitor> runningProcessMonitor) : AuroraModule
{
    public static LogitechSdkListener LogitechSdkListener { get; } = new();

    protected override async Task Initialize()
    {
        Global.Configuration.PropertyChanged += ConfigurationOnPropertyChanged;

        if (!Global.Configuration.EnableLightsyncTakeover)
        {
            Global.logger.Information("Lightsync initialization skipped because of user setting");
            LogitechSdkListener.Dispose();
            return;
        }

        Global.logger.Information("Initializing Lightsync...");
        await LogitechSdkListener.Initialize(runningProcessMonitor);
        Global.logger.Information("Initialized Lightsync");
    }

    private async void ConfigurationOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(Global.Configuration.EnableLightsyncTakeover))
        {
            return;
        }

        if (Global.Configuration.EnableLightsyncTakeover)
        {
            await LogitechSdkListener.Initialize(runningProcessMonitor);
        }
        else
        {
            LogitechSdkListener.Dispose();
        }
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