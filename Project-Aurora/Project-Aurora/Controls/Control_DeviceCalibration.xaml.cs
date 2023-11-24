using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Aurora.Devices;
using Aurora.Modules.GameStateListen;
using Aurora.Settings;
using Aurora.Utils;
using Common;
using Common.Devices;
using Common.Devices.RGBNet;

namespace Aurora.Controls;

public partial class Control_DeviceCalibration : IDisposable
{
    public Task<DeviceManager> DeviceManager { get; }

    private readonly TransparencyComponent _transparencyComponent;
    private readonly Task<IpcListener?> _ipcListener;
    private readonly SemaphoreSlim _devicesUpdated = new(0);

    private Dictionary<RemappableDevice, Color> DeviceCalibrations { get; set; } = new();

    public Control_DeviceCalibration(Task<DeviceManager> deviceManager, Task<IpcListener?> ipcListener)
    {
        DeviceManager = deviceManager;
        _ipcListener = ipcListener;

        InitializeComponent();

        _transparencyComponent = new TransparencyComponent(this, null);
    }

    public async Task Initialize()
    {
        var ipcListener = await _ipcListener;
        if (ipcListener != null)
        {
            ipcListener.AuroraCommandReceived += OnAuroraCommandReceived;
        }
    }

    private void OnAuroraCommandReceived(object? sender, string e)
    {
        var words = e.Split(Constants.StringSplit);
        if (words.Length != 2)
        {
            return;
        }

        var command = words[0];
        var json = words[1];
        switch (command)
        {
            case DeviceCommands.RemappableDevices:
                var remappableDevices = ReadDevices(json);

                DeviceCalibrations = remappableDevices.Devices.ToDictionary(
                    device => device,
                    device => Color.FromArgb(device.Calibration.ToArgb())
                );

                Dispatcher.Invoke(() =>
                {
                    DeviceList.ItemsSource = DeviceCalibrations;
                    DeviceList.Items.Refresh();
                });

                //release lock
                if (_devicesUpdated.CurrentCount > 0)
                {
                    _devicesUpdated.Release(_devicesUpdated.CurrentCount);
                }
                break;
        }
    }

    private static CurrentDevices ReadDevices(string json)
    {
        return JsonSerializer.Deserialize<CurrentDevices>(json) ?? new CurrentDevices(new List<RemappableDevice>());
    }

    private async Task RefreshLists()
    {
        await (await DeviceManager).RequestRemappableDevices();
        await _devicesUpdated.WaitAsync();
    }

    private async void Control_DeviceCalibration_OnLoaded(object sender, RoutedEventArgs e)
    {
        await RefreshLists();
    }

    public void Dispose()
    {
        _transparencyComponent.Dispose();

        var ipcListener = _ipcListener.Result;
        if (ipcListener != null)
        {
            ipcListener.AuroraCommandReceived -= OnAuroraCommandReceived;
        }
    }

    private void Control_DeviceCalibration_OnUnloaded(object sender, RoutedEventArgs e)
    {
        ConfigManager.Save(Global.DeviceConfiguration, DeviceConfig.ConfigFile);
    }
}