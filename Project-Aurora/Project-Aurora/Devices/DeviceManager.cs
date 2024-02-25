using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aurora.Modules;
using Common;
using Common.Data;
using Common.Devices;
using Common.Devices.RGBNet;
using RazerSdkReader;
using RGB.NET.Core;

namespace Aurora.Devices;

public sealed class DevicesUpdatedEventArgs(IEnumerable<DeviceContainer> deviceContainers) : EventArgs
{
    public IEnumerable<DeviceContainer> DeviceContainers { get; } = deviceContainers;
}

public sealed class DeviceManager : IDisposable
{
    public const string DeviceManagerProcess = "AuroraDeviceManager";

    private const string DeviceManagerFolder = @".\AuroraDeviceManager";
    private const string DeviceManagerExe = "AuroraDeviceManager.exe";

    private bool _disposed;

    public event EventHandler<DevicesUpdatedEventArgs>? DevicesUpdated;

    public List<DeviceContainer> DeviceContainers { get; } = [];

    private readonly Task<ChromaReader?> _rzSdkManager;
    private readonly AuroraControlInterface _auroraControlInterface;
    private readonly MemorySharedArray<SimpleColor> _sharedDeviceColor;

    private int _dmStartCount;

    private readonly MemorySharedStruct<DeviceManagerInfo> _deviceManagerInfo;
    private Process? _process;
    private bool _detached;

    private readonly byte[] _end = "\n"u8.ToArray();

    public DeviceManager(Task<ChromaReader?> rzSdkManager, AuroraControlInterface auroraControlInterface)
    {
        _rzSdkManager = rzSdkManager;
        _auroraControlInterface = auroraControlInterface;
        _sharedDeviceColor = new MemorySharedArray<SimpleColor>(Constants.DeviceLedMap, Constants.MaxKeyId);

        _deviceManagerInfo = new MemorySharedStruct<DeviceManagerInfo>(Constants.DeviceInformations);
        _deviceManagerInfo.Updated += OnDeviceManagerInfoOnUpdated;
    }

    public async Task InitializeDevices()
    {
        _dmStartCount = 0;
        await _rzSdkManager;

        AttachOrCreateProcess();
    }

    private void AttachOrCreateProcess()
    {
        _process = Process.GetProcessesByName(DeviceManagerProcess).FirstOrDefault();
        if (_process != null)
        {
            UpdateDevices();
        }
        else
        {
            StartDmProcess();
        }
    }

    private void OnDeviceManagerInfoOnUpdated(object? o, EventArgs eventArgs)
    {
        UpdateDevices();
    }

    private void UpdateDevices()
    {
        var deviceManagerInfo = _deviceManagerInfo.ReadElement();
        var deviceNames = deviceManagerInfo.DeviceNames.Split(Constants.StringSplit);

        foreach (var deviceContainer in DeviceContainers)
        {
            deviceContainer.Dispose();
        }
        DeviceContainers.Clear();
        DeviceContainers.AddRange(deviceNames.Select(deviceName =>
        {
            var device = new MemorySharedDevice(deviceName);
            return new DeviceContainer(device, this);
        }));

        DevicesUpdated?.Invoke(this, new DevicesUpdatedEventArgs(DeviceContainers.ToImmutableList()));
    }

    private void StartDmProcess()
    {
        var updaterProc = new ProcessStartInfo
        {
            FileName = Path.Combine(DeviceManagerFolder, DeviceManagerExe),
            WorkingDirectory = DeviceManagerFolder,
            ErrorDialog = true,
        };
        _process = Process.Start(updaterProc);
        _process?.WaitForExitAsync().ContinueWith(DeviceManagerClosed);
    }

    private void DeviceManagerClosed(Task processTask)
    {
        if (processTask.IsFaulted)
        {
            Global.logger.Error(processTask.Exception, "Device Manager closed unexpectedly");
        }

        if (_dmStartCount++ > 4)
        {
            Global.logger.Error("Device manager failed too much. Stopping initializing");
            _auroraControlInterface.ShowErrorNotification("Device Manager cannot be started. Check logs for more information");
            return;
        }

        if (_process != null)
        {
            AttachOrCreateProcess();
        }
    }

    public async Task ShutdownDevices()
    {
        if (_detached)
        {
            return;
        }
        _process ??= Process.GetProcessesByName(DeviceManagerProcess).FirstOrDefault();
        if (_process != null)
        {
            var process = _process;
            _process = null;

            if (await IsDeviceManagerUp())
            {
                await SendCommand(DeviceCommands.Quit);
            }
            else
            {
                process.Kill();
            }
            await process.WaitForExitAsync();
            DevicesUpdated?.Invoke(this, new DevicesUpdatedEventArgs([]));
        }
    }

    public async Task ResetDevices()
    {
        await ShutdownDevices();
        await InitializeDevices();
    }

    public void Detach()
    {
        _detached = true;
    }

    public void UpdateDevices(IReadOnlyDictionary<DeviceKeys, SimpleColor> keyColors)
    {
        _sharedDeviceColor.WriteDictionary(keyColors);
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }
        _disposed = true;
        _deviceManagerInfo.Dispose();
        _sharedDeviceColor.Dispose();
    }

    public async Task BlinkRemappableKey(RemappableDevice remappableDevice, LedId led)
    {
        if (_process == null)
        {
            return;
        }

        var parameters = remappableDevice.DeviceId + Constants.StringSplit + (int)led;
        var command = DeviceCommands.Blink + Constants.StringSplit + parameters;
        await SendCommand(command);
    }

    public async Task RemapKey(string deviceId, LedId led, DeviceKeys? newKey)
    {
        if (_process == null)
        {
            return;
        }

        if (newKey == null)
        {
            var parameters = deviceId + Constants.StringSplit + (int)led;
            var command = DeviceCommands.Unmap + Constants.StringSplit + parameters;

            await SendCommand(command);
        }
        else
        {
            var parameters = deviceId + Constants.StringSplit + (int)led + Constants.StringSplit + (int)newKey;
            var command = DeviceCommands.Remap + Constants.StringSplit + parameters;

            await SendCommand(command);
        }
    }

    public async Task RequestRemappableDevices()
    {
        await SendCommand( DeviceCommands.Share);
    }

    public async Task EnableDevice(string deviceDeviceName)
    {
        var command = DeviceCommands.Enable + Constants.StringSplit + deviceDeviceName;
        await SendCommand( command);
    }

    public async Task DisableDevice(string deviceDeviceName)
    {
        var command = DeviceCommands.Disable + Constants.StringSplit + deviceDeviceName;
        await SendCommand( command);
    }

    public async Task Recalibrate(string deviceName, SimpleColor color)
    {
        var command = DeviceCommands.Recalibrate + Constants.StringSplit + deviceName + Constants.StringSplit + color.ToArgb();
        await SendCommand( command);
    }

    public async Task<bool> IsDeviceManagerUp()
    {
        var runningProcessMonitor = await ProcessesModule.RunningProcessMonitor;
        if (!runningProcessMonitor.IsProcessRunning(DeviceManagerExe.ToLowerInvariant()))
        {
            return false;
        }
        
        for (var i = 0; i < 5; i++)
        {
            var pipeOpen = await IsPipeOpen();
            if (pipeOpen)
            {
                return true;
            }
            await Task.Delay(200);
        }
        return false;
    }

    private Task<bool> IsPipeOpen()
    {
        var tcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
        Task.Run(() =>
        {
            var dmUp = File.Exists(@"\\.\pipe\" + Constants.DeviceManagerPipe);
            tcs.SetResult(dmUp);
        });
        return tcs.Task;
    }

    private async Task SendCommand(string command)
    {
        await SendCommand(Encoding.UTF8.GetBytes(command));
    }

    private async Task SendCommand(byte[] command)
    {
        await using var client = new NamedPipeClientStream(".", Constants.DeviceManagerPipe, PipeDirection.Out, PipeOptions.None);
        await client.ConnectAsync(1000);
        if (!client.IsConnected)
            throw new InvalidOperationException("Connection to DeviceManager failed");
        
        client.Write(command, 0, command.Length);
        client.Write(_end, 0, _end.Length);
        
        client.Flush();
    }
}