using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using Aurora.Devices;
using Aurora.Modules.GameStateListen;

namespace Aurora;

public sealed class AuroraControlInterface(Task<DeviceManager> deviceManager, Task<IpcListener?> listener)
{
    public async Task Initialize()
    {
        var ipcListener = await listener;
        if (ipcListener != null)
        {
            ipcListener.AuroraCommandReceived += OnAuroraCommandReceived;
        }
    }

    private void OnAuroraCommandReceived(object? sender, string e)
    {
        Global.logger.Debug("Received command: {Command}", e);
        switch (e)
        {
            case "shutdown":
                ShutdownDevices().Wait();
                ExitApp();
                break;
        }
    }
    
    public void ExitApp()
    {
        //to only shutdown Aurora itself
        deviceManager.Result.Detach();
        Application.Current.Shutdown();
    }

    public async Task RestartDevices()
    {
        await (await deviceManager).ResetDevices();
    }

    public async Task ShutdownDevices()
    {
        await (await deviceManager).ShutdownDevices();
    }

    public void RestartAurora()
    {
        //so that we don't restart device manager
        deviceManager.Result.Detach();

        var auroraPath = Path.Combine(Global.ExecutingDirectory, "Aurora.exe");

        var currentProcess = Environment.ProcessId;
        var minimizedArg = Application.Current?.MainWindow?.Visibility == Visibility.Visible ? "" : " -minimized";
        Process.Start(new ProcessStartInfo
        {
            FileName = auroraPath,
            Arguments = $"-restart {currentProcess}{minimizedArg}"
        });

        ExitApp();
    }
}