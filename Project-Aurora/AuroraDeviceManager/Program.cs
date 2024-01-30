using AuroraDeviceManager;
using Common.Devices;
using AuroraDeviceManager.Devices;
using AuroraDeviceManager.Utils;
using Common;
using Common.Data;
using Color = System.Drawing.Color;

{
    const uint shutdownNotorietry = 0x00000001;
    //with this, DeviceManager shutdown will be called after Aurora, default priority is 280
    Kernel32.SetProcessShutdownParameters(0x210, shutdownNotorietry);
}

Global.Initialize();

AppDomain.CurrentDomain.UnhandledException += (_, eventArgs) =>
{
    Global.Logger.Fatal((Exception)eventArgs.ExceptionObject, "Device Manager crashed");
    if (Global.Logger is IDisposable logger)
    {
        logger.Dispose();
    }
};

Global.Logger.Information("Loading AuroraDeviceManager");
var deviceManager = new DeviceManager();

//Load config
Global.Logger.Information("Loading Configuration");
var configManager = new ConfigManager(deviceManager);
await configManager.Load();

var endTaskSource = new TaskCompletionSource();
var pipe = new AuroraPipe(deviceManager);
pipe.Shutdown += (_, _) => endTaskSource.TrySetResult();

var colors = new MemorySharedArray<SimpleColor>(Constants.DeviceLedMap);

var deviceKeys = new Dictionary<DeviceKeys, Color>();

colors.Updated += OnColorsOnUpdated;

await deviceManager.InitializeDevices();
await endTaskSource.Task;

Global.Logger.Information("Closing DeviceManager");
colors.Updated -= OnColorsOnUpdated;
Stop();
return;

void OnColorsOnUpdated(object? o, EventArgs eventArgs)
{
    for (var i = 0; i < colors.Count; i++)
    {
        var color = colors.ReadElement(i);
        deviceKeys[(DeviceKeys)i] = (Color)color;
    }

    deviceManager.UpdateDevices(deviceKeys);
}

void Stop()
{
    App.Closing = true;

    deviceManager.ShutdownDevices().Wait(5000);
    deviceManager.Dispose();

    endTaskSource.TrySetResult();
}
