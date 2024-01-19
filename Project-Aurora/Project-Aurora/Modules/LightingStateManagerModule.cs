using System.Threading.Tasks;
using Aurora.Devices;
using Aurora.Modules.GameStateListen;
using Aurora.Modules.ProcessMonitor;
using Aurora.Profiles;
using Aurora.Settings;
using Lombok.NET;

namespace Aurora.Modules;

public sealed partial class LightingStateManagerModule(
    Task<PluginManager> pluginManager,
    Task<IpcListener?> listener,
    Task<AuroraHttpListener?> httpListener,
    Task<DeviceManager> deviceManager,
    Task<ActiveProcessMonitor> activeProcessMonitor,
    Task<RunningProcessMonitor> runningProcessMonitor
) : AuroraModule
{
    private static readonly TaskCompletionSource<LightingStateManager> TaskSource = new(TaskCreationOptions.RunContinuationsAsynchronously);

    private LightingStateManager? _manager;

    public static Task<LightingStateManager> LightningStateManager => TaskSource.Task;

    protected override async Task Initialize()
    {
        Global.logger.Information("Loading Applications");
        var lightingStateManager = new LightingStateManager(pluginManager, listener, deviceManager, activeProcessMonitor, runningProcessMonitor);
        _manager = lightingStateManager;
        Global.LightingStateManager = lightingStateManager;
        await lightingStateManager.Initialize();

        TaskSource.SetResult(lightingStateManager);

        var ipcListener = await listener;
        if (ipcListener != null)
        {
            ipcListener.NewGameState += lightingStateManager.GameStateUpdate;
            ipcListener.WrapperConnectionClosed += lightingStateManager.ResetGameState;
        }

        var httpListener1 = await httpListener;
        if (httpListener1 != null)
        {
            httpListener1.NewGameState += lightingStateManager.GameStateUpdate;
        }
        await lightingStateManager.InitUpdate();
    }

    [Async]
    public override void Dispose()
    {
        _manager?.Dispose();
        Global.LightingStateManager = null;
        _manager = null;
    }
}