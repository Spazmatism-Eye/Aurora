using System;
using System.Windows;
using AuroraRgb.Utils;
using Common.Devices;

namespace AuroraRgb.Settings.Controls;

/// <summary>
/// Interaction logic for Window_VariableRegistryEditor.xaml
/// </summary>
public sealed partial class Window_VariableRegistryEditor : IDisposable
{
    private readonly TransparencyComponent _transparencyComponent;
    private readonly DeviceConfig _deviceConfig;

    public Window_VariableRegistryEditor(DeviceConfig deviceConfig)
    {
        _deviceConfig = deviceConfig;
        InitializeComponent();

        _transparencyComponent = new TransparencyComponent(this, null);
    }

    public void Dispose()
    {
        _transparencyComponent.Dispose();
    }

    private void Window_VariableRegistryEditor_OnLoaded(object sender, RoutedEventArgs e)
    {
        VarRegistryEditor.VarRegistrySource = _deviceConfig.VarRegistry;
    }

    private void Window_VariableRegistryEditor_OnUnloaded(object sender, RoutedEventArgs e)
    {
        ConfigManager.Save(_deviceConfig);
        Dispose();
    }
}