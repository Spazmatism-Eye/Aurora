using Aurora.Settings;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Aurora.Devices;
using Aurora.Settings.Controls;
using Common.Devices;

namespace Aurora.Controls;

/// <summary>
/// Interaction logic for Control_DeviceItem.xaml
/// </summary>
public partial class Control_DeviceItem
{
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public static readonly DependencyProperty DeviceProperty = DependencyProperty.Register(
        nameof(Device), typeof(DeviceContainer), typeof(Control_DeviceItem), new PropertyMetadata(DevicePropertyUpdated)
    );

    private static void DevicePropertyUpdated(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var deviceItem = (Control_DeviceItem)d;

        if (e.OldValue is DeviceContainer oldValue)
        {
            oldValue.Device.Updated -= deviceItem.OnDeviceOnUpdated;
        }

        deviceItem.Device.Device.Updated += deviceItem.OnDeviceOnUpdated;
    }

    private readonly Timer _updateControlsTimer = new(1000);

    private bool _deviceRunning;

    public DeviceContainer Device
    {
        get => (DeviceContainer)GetValue(DeviceProperty);
        set => SetValue(DeviceProperty, value);
    }

    public Control_DeviceItem()
    {
        InitializeComponent();

        _updateControlsTimer.Elapsed += Update_controls_timer_Elapsed;
    }

    private void Update_controls_timer_Elapsed(object? sender, EventArgs e)
    {
        if (IsVisible)
        {
            RequestUpdate();
        }
    }

    private void RequestUpdate()
    {
        Dispatcher.BeginInvoke(() =>
        {
            var memorySharedDevice = Device.Device;
            Task.Run(() => { memorySharedDevice.RequestUpdate(); });
        }, DispatcherPriority.Background);
    }

    private void btnToggleOnOff_Click(object? sender, EventArgs e)
    {
        BtnStart.Content = "Working...";
        BtnStart.IsEnabled = false;
        var device = Device;
        if (device.Device.IsInitialized)
        {
            Task.Run(async () =>
            {
                await device.DisableDevice();
            });
        }
        else
        {
            Task.Run(async () =>
            {
                await device.EnableDevice();
            });
        }
    }

    private void btnToggleEnableDisable_Click(object? sender, EventArgs e)
    {
        var deviceEnabled = !Global.DeviceConfiguration.EnabledDevices.Contains(Device.Device.DeviceName);
        if (deviceEnabled)
        {
            Global.DeviceConfiguration.EnabledDevices.Add(Device.Device.DeviceName);
        }
        else
        {
            Global.DeviceConfiguration.EnabledDevices.Remove(Device.Device.DeviceName);
        }
        UpdateDynamic();

        if (_deviceRunning)
        {
            BtnStart.Content = "Working...";
            BtnStart.IsEnabled = false;
            BtnEnable.IsEnabled = false;
            
            var device = Device;
            Task.Run(async () =>
            {
                await device.DisableDevice();
            });
        }
        Task.Run(() =>
        {
            ConfigManager.Save(Global.DeviceConfiguration, DeviceConfig.ConfigFile);
        });
    }

    private void UserControl_Loaded(object? sender, EventArgs e)
    {
        Dispatcher.BeginInvoke(() =>
        {
            try
            {
                UpdateStatic();
                UpdateDynamic();
            }
            catch (Exception ex)
            {
                Global.logger.Warning(ex, "DeviceItem update error:");
            }
        }, DispatcherPriority.Background);
    }

    private void Control_DeviceItem_OnUnloaded(object? sender, EventArgs e)
    {
        Device.Device.Updated -= OnDeviceOnUpdated;
        _updateControlsTimer.Stop();
    }

    private void OnDeviceOnUpdated(object? o, EventArgs eventArgs)
    {
        Dispatcher.BeginInvoke(() =>
        {
            try
            {
                UpdateDynamic();
            }
            catch (Exception ex)
            {
                Global.logger.Warning(ex, "DeviceItem update error:");
            }
        }, Background);
    }

    private void UpdateStatic()
    {
        Beta.Visibility = Device.Device.Tooltips.Beta ? Visibility.Visible : Visibility.Hidden;

        var infoTooltip = Device.Device.Tooltips.Info;
        if (infoTooltip != null)
        {
            InfoTooltip.HintTooltip = infoTooltip;
            InfoTooltip.Visibility = Visibility.Visible;
        }
        else
        {
            InfoTooltip.Visibility = Visibility.Hidden;
        }

        var sdkLink = Device.Device.Tooltips.SdkLink;
        if (sdkLink != null)
        {
            SdkLink.MouseDoubleClick -= SdkLink_Clicked;
            SdkLink.MouseDoubleClick += SdkLink_Clicked;
            SdkLink.Visibility = Visibility.Visible;
        }
        else
        {
            SdkLink.MouseDoubleClick -= SdkLink_Clicked;
            SdkLink.Visibility = Visibility.Hidden;
        }

        Recommended.Visibility = Device.Device.Tooltips.Recommended ? Visibility.Visible : Visibility.Hidden;

        BtnOptions.IsEnabled = Device.Device.RegisteredVariables.Count != 0;
        DeviceName.Text = Device.Device.DeviceName;
    }

    private void UpdateDynamic()
    {
        if (Device.Device.isDoingWork)
        {
            _deviceRunning = false;
            BtnStart.Content = "Working...";
            BtnStart.IsEnabled = false;
            BtnEnable.IsEnabled = false;
            _updateControlsTimer.Start();
        }
        else if (Device.Device.IsInitialized)
        {
            _deviceRunning = true;
            BtnStart.Content = "Stop";
            BtnStart.IsEnabled = true;
            BtnEnable.IsEnabled = true;
            _updateControlsTimer.Start();
        }
        else
        {
            _deviceRunning = false;
            BtnStart.Content = "Start";
            BtnStart.IsEnabled = true;
            BtnEnable.IsEnabled = true;
        }

        DeviceDetails.Text = Device.Device.DeviceDetails;
        DevicePerformance.Text = Device.Device.DeviceUpdatePerformance;

        if (!Global.DeviceConfiguration.EnabledDevices.Contains(Device.Device.DeviceName))
        {
            BtnEnable.Content = "Enable";
            BtnStart.IsEnabled = false;
        }
        else if (!Device.Device.isDoingWork)
        {
            BtnEnable.Content = "Disable";
        }
    }

    private void SdkLink_Clicked(object? sender, MouseButtonEventArgs e)
    {
        var sdkLink = Device.Device.Tooltips.SdkLink;
        if (sdkLink != null)
        {
            System.Diagnostics.Process.Start("explorer", sdkLink);
        }
    }

    private void btnViewOptions_Click(object? sender, RoutedEventArgs e)
    {
        var optionsWindow = new Window_VariableRegistryEditor
        {
            Title = $"{Device.Device.DeviceName} - Options",
            SizeToContent = SizeToContent.WidthAndHeight,
            VarRegistryEditor =
            {
                RegisteredVariables = Device.Device.RegisteredVariables
            }
        };
        optionsWindow.Closing += (_, _) => { ConfigManager.Save(Global.DeviceConfiguration, DeviceConfig.ConfigFile); };

        optionsWindow.ShowDialog();
    }
}