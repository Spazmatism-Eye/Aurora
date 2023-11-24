using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows;
using Aurora.Devices;
using Common;
using Common.Devices.RGBNet;
using Common.Utils;

namespace Aurora.Controls;

public partial class Control_DeviceCalibrationItem
{
    private readonly SingleConcurrentThread _worker;
    
    public Task<DeviceManager>? DeviceManager
    {
        get => GetValue(DeviceManagerProperty) as Task<DeviceManager>;
        set => SetValue(DeviceManagerProperty, value);
    }
    
    public static readonly DependencyProperty DeviceManagerProperty =
        DependencyProperty.Register(
            nameof(DeviceManager), // This is wrong
            typeof(Task<DeviceManager>),
            typeof(Control_DeviceCalibrationItem));
    
    private KeyValuePair<RemappableDevice, Color> DeviceColor => (KeyValuePair<RemappableDevice, Color>) DataContext;

    private DeviceManager? _deviceManager;
    private SimpleColor _color = SimpleColor.White;
    private string _deviceKey = "";

    public Control_DeviceCalibrationItem()
    {
        _worker = new SingleConcurrentThread("Device Calibration", () => { WorkerOnDoWork().Wait(); });

        InitializeComponent();
    }

    private async void ColorPicker_OnSelectedColorChanged(object? sender, RoutedPropertyChangedEventArgs<System.Windows.Media.Color?> e)
    {
        if (DeviceManager == null)
        {
            return;
        }
        
        var color = e.NewValue.GetValueOrDefault();
        _color = new SimpleColor(color.R, color.G, color.B);

        _deviceManager = await DeviceManager;
        _deviceKey = DeviceColor.Key.DeviceId;

        Global.DeviceConfiguration.DeviceCalibrations[_deviceKey] = _color;
        _worker.Trigger();
    }

    private async Task WorkerOnDoWork()
    {
        if (_deviceManager == null)
        {
            return;
        }

        await _deviceManager.Recalibrate(_deviceKey, _color);
    }

    private void ResetDevice_OnClick(object sender, RoutedEventArgs e)
    {
        ColorPicker.SelectedColor = System.Windows.Media.Color.FromArgb(255, 255, 255, 255);
    }
}