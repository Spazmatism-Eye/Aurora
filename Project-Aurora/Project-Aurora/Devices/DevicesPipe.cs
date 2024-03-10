using System;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.Devices;
using Common.Devices.RGBNet;
using RGB.NET.Core;

namespace Aurora.Devices;

public sealed class DevicesPipe
{
    private readonly byte[] _end = "\n"u8.ToArray();

    public Task<bool> IsPipeOpen()
    {
        var tcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
        Task.Run(() =>
        {
            var dmUp = File.Exists(@"\\.\pipe\" + Constants.DeviceManagerPipe);
            tcs.SetResult(dmUp);
        });
        return tcs.Task;
    }

    public async Task Shutdown()
    {
        await SendCommand(DeviceCommands.Quit);
    }

    public async Task BlinkRemappableKey(RemappableDevice remappableDevice, LedId led)
    {
        var parameters = remappableDevice.DeviceId + Constants.StringSplit + (int)led;
        var command = DeviceCommands.Blink + Constants.StringSplit + parameters;
        await SendCommand(command);
    }

    public async Task RemapKey(string deviceId, LedId led, DeviceKeys? newKey)
    {
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

    async Task SendCommand(string command)
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