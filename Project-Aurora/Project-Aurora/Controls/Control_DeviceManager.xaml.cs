using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Aurora.Devices;
using Aurora.Modules;
using Aurora.Modules.GameStateListen;
using Aurora.Modules.ProcessMonitor;

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

    private async void DeviceManagerOnDevicesUpdated(object? sender, EventArgs e)
    {
        await UpdateControls();
    }

    private async void Control_DeviceManager_Loaded(object? sender, RoutedEventArgs e)
    {
        var deviceManager = await _deviceManager;
        deviceManager.DevicesUpdated += DeviceManagerOnDevicesUpdated;

        await Task.Run(UpdateControls).ConfigureAwait(false);
    }

    private async void Control_DeviceManager_Unloaded(object? sender, RoutedEventArgs e)
    {
        var deviceManager = await _deviceManager;
        deviceManager.DevicesUpdated -= DeviceManagerOnDevicesUpdated;
    }

    private async Task UpdateControls()
    {
        var deviceManager = await _deviceManager;
        var isDeviceManagerUp = await deviceManager.IsDeviceManagerUp();
        var deviceContainers = isDeviceManagerUp ? deviceManager.DeviceContainers : [];
        Dispatcher.BeginInvoke(() =>
        {
            NoDevManTextBlock.Visibility = isDeviceManagerUp ? Visibility.Collapsed : Visibility.Visible;
            if (ReferenceEquals(LstDevices.ItemsSource, deviceContainers))
            {
                LstDevices.Items.Refresh();
            }
            else
            {
                LstDevices.ItemsSource = deviceContainers;
            }
        }, DispatcherPriority.Loaded);

        if (!isDeviceManagerUp)
        {
            await WaitForDeviceManager();
        }
    }

    private async Task WaitForDeviceManager()
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

            await UpdateControls();
        }
    }

    private async void btnRestartAll_Click(object? sender, RoutedEventArgs e)
    {
        var devManager = await _deviceManager;
        await devManager.ShutdownDevices();
        await devManager.InitializeDevices();
        await UpdateControls();
    }

    private async void btnCalibrate_Click(object? sender, RoutedEventArgs e)
    {
        var calibration = new Control_DeviceCalibration(_deviceManager, _ipcListener);
        await calibration.Initialize();
        calibration.Show();
    }
}