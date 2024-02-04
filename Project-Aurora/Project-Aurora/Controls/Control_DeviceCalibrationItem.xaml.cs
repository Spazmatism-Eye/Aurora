using System.Threading.Tasks;
using System.Windows;
using Aurora.Devices;
using Common;
using Common.Devices;
using Common.Devices.RGBNet;
using Common.Utils;
using MediaColor = System.Windows.Media.Color;

namespace Aurora.Controls;

public partial class Control_DeviceCalibrationItem
{
    private readonly SingleConcurrentThread _worker;

    private readonly DeviceManager _deviceManager;
    private readonly DeviceConfig _deviceConfig;

    private SimpleColor _color;
    private readonly string _deviceKey;

    public Control_DeviceCalibrationItem(DeviceManager deviceManager, DeviceConfig deviceConfig, RemappableDevice device, SimpleColor color)
    {
        _deviceManager = deviceManager;
        _deviceConfig = deviceConfig;
        _deviceKey = device.DeviceId;
        _color = color;

        _worker = new SingleConcurrentThread("Device Calibration", () => { WorkerOnDoWork().Wait(); });

        InitializeComponent();

        DeviceText.Text = device.DeviceSummary;
        ColorPicker.SelectedColor = MediaColor.FromArgb(_color.A, _color.R, _color.G, _color.B);
    }

    private void ColorPicker_OnSelectedColorChanged(object? sender, RoutedPropertyChangedEventArgs<System.Windows.Media.Color?> e)
    {
        var color = e.NewValue.GetValueOrDefault();
        _color = new SimpleColor(color.R, color.G, color.B);

        _deviceConfig.DeviceCalibrations[_deviceKey] = _color;
        _worker.Trigger();
    }

    private async Task WorkerOnDoWork()
    {
        await _deviceManager.Recalibrate(_deviceKey, _color);
    }

    private void ResetDevice_OnClick(object sender, RoutedEventArgs e)
    {
        ColorPicker.SelectedColor = MediaColor.FromArgb(255, 255, 255, 255);
    }
}