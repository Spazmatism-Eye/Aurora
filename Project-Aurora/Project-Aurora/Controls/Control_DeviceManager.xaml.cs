using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Aurora.Devices;
using Aurora.Modules;
using Aurora.Modules.GameStateListen;
using Aurora.Modules.ProcessMonitor;
using Aurora.Settings;
using Common.Devices;

namespace Aurora.Controls;

/// <summary>
/// Interaction logic for Control_DeviceManager.xaml
/// </summary>
public partial class Control_DeviceManager
{
    private readonly Task<DeviceManager> _deviceManager;
    private readonly Task<IpcListener?> _ipcListener;

    public Control_DeviceManager(Task<DeviceManager> deviceManager, Task<IpcListener?> ipcListener)
    {
        _deviceManager = deviceManager;
        _ipcListener = ipcListener;

        InitializeComponent();
    }

    private async void Control_DeviceManager_Loaded(object? sender, RoutedEventArgs e)
    {
        if (!IsVisible)
        {
            return;
        }

        await LoadDeviceManager();
    }

    private async Task LoadDeviceManager()
    {
        var deviceConfig = await ConfigManager.LoadDeviceConfig();

        await Task.Run(() => UpdateControls(deviceConfig)).ConfigureAwait(false);
        
        var deviceManager = await _deviceManager;
        deviceManager.DevicesUpdated += DeviceManagerOnDevicesUpdated();
        Loaded += (_, _) => deviceManager.DevicesUpdated -= DeviceManagerOnDevicesUpdated();
        Unloaded += (_, _) => deviceManager.DevicesUpdated -= DeviceManagerOnDevicesUpdated();
        return;

        EventHandler DeviceManagerOnDevicesUpdated()
        {
            return ManagerOnDevicesUpdated;

            async void ManagerOnDevicesUpdated(object? o, EventArgs eventArgs)
            {
                await UpdateControls(deviceConfig);
            }
        }
    }

    private async Task UpdateControls(DeviceConfig deviceConfig)
    {
        var deviceManager = await _deviceManager;
        var isDeviceManagerUp = await deviceManager.IsDeviceManagerUp();
        var deviceContainers = isDeviceManagerUp ? deviceManager.DeviceContainers : [];
        Dispatcher.BeginInvoke(() =>
        {
            LstDevices.Children.Clear();
            NoDevManTextBlock.Visibility = isDeviceManagerUp ? Visibility.Collapsed : Visibility.Visible;
        });
        foreach (var deviceContainer in deviceContainers)
        {
            Dispatcher.BeginInvoke(() =>
            {
                var controlDeviceItem = new Control_DeviceItem(deviceConfig, deviceContainer);
                var listViewItem = new ListViewItem
                {
                    Content = controlDeviceItem,
                };
                LstDevices.Children.Add(listViewItem);
            }, DispatcherPriority.Loaded);
        }

        if (!isDeviceManagerUp)
        {
            await WaitForDeviceManager(deviceConfig);
        }
    }

    private async Task WaitForDeviceManager(DeviceConfig deviceConfig)
    {
        var runningProcessMonitor = await ProcessesModule.RunningProcessMonitor;
        runningProcessMonitor.ProcessStarted += RunningProcessMonitorOnRunningProcessesChanged;
        return;

        async void RunningProcessMonitorOnRunningProcessesChanged(object? sender, ProcessStarted e)
        {
            if (e.ProcessName != DeviceManager.DeviceManagerProcess)
            {
                return;
            }

            await Task.Delay(1000); // wait for pipe
            runningProcessMonitor.ProcessStarted -= RunningProcessMonitorOnRunningProcessesChanged;

            await UpdateControls(deviceConfig);
        }
    }

    private async void btnRestartAll_Click(object? sender, RoutedEventArgs e)
    {
        var devManager = await _deviceManager;
        await devManager.ShutdownDevices();
        await devManager.InitializeDevices();
    }

    private void btnCalibrate_Click(object? sender, RoutedEventArgs e)
    {
        var calibration = new Control_DeviceCalibration(_deviceManager, _ipcListener);
        calibration.Show();
    }
}