using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Aurora.Devices;
using Aurora.Modules.GameStateListen;

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

    public async Task Initialize()
    {
        var deviceManager = await _deviceManager;
        deviceManager.DevicesUpdated += async (_, _) =>
        {
            await UpdateControls();
        };
    }

    private void Control_DeviceManager_Loaded(object? sender, RoutedEventArgs e)
    {
        Task.Run(UpdateControls);
    }

    private async Task UpdateControls()
    {
        var deviceManager = await _deviceManager;
        var isDeviceManagerUp = await deviceManager.IsDeviceManagerUp();
        var deviceContainers = isDeviceManagerUp ? deviceManager.DeviceContainers : new List<DeviceContainer>();
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